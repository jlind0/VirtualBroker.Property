using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using VirtualBroker.Property.Business.Core;
using VirtualBroker.Property.Core;
using VirtualBroker.Property.Data.Core;

namespace VirtualBroker.Property.ViewModels
{
    public class ImpersonatorViewModel : ManageEntityViewModel<Guid, User, IUserBusinessFacade>
    {
        public ReactiveCommand<Guid, Unit> Impersonate { get; }
        public ReactiveCommand<Unit, Unit> EndImpersonation { get; }
        public ReactiveCommand<Unit, Guid?> Load { get; }
        protected IContextFactory ContextFactory { get; }
        protected NavigationManager NavMan { get; }
        private User? impersonatedUser;
        public User? ImpersonatedUser
        {
            get => impersonatedUser;
            set => this.RaiseAndSetIfChanged(ref impersonatedUser, value);
        }
        public ImpersonatorViewModel(NavigationManager manage, IContextFactory factory, IUserBusinessFacade facade, ILogger<ManageEntityViewModel<Guid, User, IUserBusinessFacade>> logger) : base(facade, logger)
        {
            ContextFactory = factory;
            NavMan = manage;
            Impersonate = ReactiveCommand.CreateFromTask<Guid>(DoImpersonate);
            EndImpersonation = ReactiveCommand.CreateFromTask(DoEndImpersonation);
            Load = ReactiveCommand.CreateFromTask<Guid?>(DoLoadImpersonation);
        }
        protected override async Task<Expression<Func<User, bool>>?> GetBaseFilterCondition()
        {
            var realUserId = await Facade.GetCurrentUserId(fetchTrueUserId: true);
            if (realUserId == null)
                throw new InvalidOperationException();
            return q => q.Id == realUserId;
        }
        protected override async Task<RepositoryResultSet<Guid, User>?> DoFetch(DataGridRequest<Guid, User> request, CancellationToken token)
        {
            var impersonated = await DoLoadImpersonation(token);
            var fetch = await base.DoFetch(request, token);
            if (fetch == null)
                return null;
            fetch.Entities = fetch.Entities.Where(e => e.Id != impersonated).ToArray();
            return fetch;

        }
        protected async Task<Guid?> DoLoadImpersonation(CancellationToken token)
        {
            var impersonated = await ContextFactory.GetImpersonatedUser();
            if (impersonated != null)
                ImpersonatedUser = await Facade.GetByID(impersonated.Value, token: token);
            return impersonated;
        }
        protected async Task DoImpersonate(Guid id, CancellationToken token)
        {
            await ContextFactory.SetImpersonatedUser(id);
            NavMan.NavigateTo("/resume/profile");
            NavMan.Refresh(true);
        }
        protected async Task DoEndImpersonation(CancellationToken token)
        {
            await ContextFactory.SetImpersonatedUser(null);
            ImpersonatedUser = null;
            NavMan.Refresh(true);
        }
    }
}
