using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtualBroker.Property.Business.Core;
using VirtualBroker.Property.Core;

namespace VirtualBroker.Property.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RolesController : ControllerBase
    {
        protected IBusinessRepositoryFacade<Role, Guid> RoleFacade { get; }
        protected IUserBusinessFacade UserFacade { get; }
        protected ILogger Logger { get; }
        public RolesController(IBusinessRepositoryFacade<Role, Guid> roleFacade, IUserBusinessFacade userFacade, ILogger<RolesController> logger)
        {
            RoleFacade = roleFacade;
            UserFacade = userFacade;
            Logger = logger;
        }
        public class RoleBody
        {
            public string extension_Roles { get; set; } = null!;
            public string version { get; set; } = "1.0.0";
            public string action { get; set; } = "continue";
        }
        [HttpPost("/permissions")]
        public async Task<RoleBody> GetUserPermissions([FromBody] JObject message, CancellationToken token = default)
        {
            try
            {
                await using (var uow = UserFacade.CreateUnitOfWork())
                {
                    Logger.LogInformation("User Permissions Hydration Invoked");
                    Logger.LogInformation(message.ToString(Formatting.None)); // Serializing with Newtonsoft.Json

                    // Parsing the JSON using JObject
                    var userId = (message["objectId"] ?? throw new InvalidDataException()).ToString();
                    var user = await UserFacade.GetByObjectIdAsync(userId, properties: p => p.Roles, work: uow, token: token);
                    RepositoryResultSet<Guid, Role>? roles = null;
                    if (user == null)
                    {
                        user = new User()
                        {
                            ObjectId = userId,
                            LastName = message["surname"]?.ToString(),
                            FirstName = message["givenName"]?.ToString(),
                            EmailAddress = (message["email"] ?? throw new InvalidDataException()).ToString()
                        };
                        await UserFacade.Add(user, work: uow, token: token);
                        await uow.Commit(token);
                    }
                    else
                    {
                        var roleIds = user.Roles.Select(u => u.Id).ToArray();
                        roles = await RoleFacade.Get(work: uow, filter: q => roleIds.Any(r => r == q.Id));
                    }

                    string? rolesStr = null;
                    if (roles != null)
                        rolesStr = string.Join(',', roles.Entities.Select(ur => ur.Name));

                    return new RoleBody()
                    {
                        extension_Roles = rolesStr ?? string.Empty
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
