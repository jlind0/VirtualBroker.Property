using Invio.Extensions.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;
using VirtualBroker.Property.Core;
using VirtualBroker.Property.Data.Core;
using VirtualBroker.Property.Data;
using VirtualBroker.Property.Business.Core;
using VirtualBroker.Property.Business;
using VirtualBroker.Property.Web.Shared;
using VirtualBroker.Property.ViewModels.Admin;
using VirtualBroker.Property.ViewModels;
using VirtualBroker.Property.Web.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearerQueryStringAuthentication()
    .AddMicrosoftIdentityWebApi(builder.Configuration)
                    .EnableTokenAcquisitionToCallDownstreamApi()
                        .AddSessionTokenCaches();
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
   .AddMicrosoftIdentityWebApp(options =>
   {
       builder.Configuration.Bind("AzureAd", options);
       options.Events.OnSignedOutCallbackRedirect = context =>
       {
           context.HttpContext.Session.Clear();
           return Task.CompletedTask;
       };
       options.Events.OnRemoteFailure = context =>
       {
           if (context.Failure.Message.Contains("AADB2C90118"))
           {
               // Redirect to Password Reset policy
               var resetPasswordUrl = "https://lindinnovation.b2clogin.com/tfp/lindinnovation.onmicrosoft.com/B2C_1_pswreset/oauth2/v2.0/authorize"
                                    + $"?client_id={Uri.EscapeDataString(options.ClientId)}"
                                    + $"&redirect_uri=https://{context.Request.Host}{Uri.EscapeDataString(options.CallbackPath)}"
                                    + "&response_mode=query"
                                    + "&response_type=code"
                                    + "&scope=openid";
               context.Response.Redirect(resetPasswordUrl);
               context.HandleResponse();
           }
           else if (context.Failure.Message.Contains("State"))
           {
               context.Response.Redirect("/");
               context.HandleResponse();
           }
           return Task.CompletedTask;
       };
   }).EnableTokenAcquisitionToCallDownstreamApi();

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization();
builder.Services.AddMemoryCache();
builder.Services.AddAuthorization();
builder.Services.AddTokenAcquisition();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddMvc().AddNewtonsoftJson();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromHours(10); // Adjust as needed
})
    .AddHubOptions(options =>
    {
        options.MaximumReceiveMessageSize = 1024 * 1024 * 10; // 10 MB
    })
    .AddMicrosoftIdentityConsentHandler();
builder.Services.AddMudServices();
builder.Services.AddBlazorBootstrap();

var connectionString = builder.Configuration.GetConnectionString("VirtualBroker");
builder.Services.AddDbContext<VirtualBrokersDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
builder.Services.AddScoped<IContextFactory, ContextFactory>();
builder.Services.AddScoped<IRepository<Role, Guid>, Repository<Role, Guid>>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IBusinessRepositoryFacade<Role, Guid>, BusinessRepositoryFacade<Role, Guid, IRepository<Role, Guid>>>();
builder.Services.AddScoped<IUserBusinessFacade, UserBusinessFacade>();
builder.Services.AddScoped<IRoleBusinessFacade, RoleBusinessFacade>();
builder.Services.AddTransient<AlertView.AlertViewModel>();
builder.Services.AddTransient<UserProfileLoaderViewModel>();
builder.Services.AddTransient<UserBarLoaderViewModel>();
builder.Services.AddTransient<ImpersonatorViewModel>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust timeout as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // For GDPR compliance
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use HTTPS
});
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseMiddleware<RolePopulationMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
