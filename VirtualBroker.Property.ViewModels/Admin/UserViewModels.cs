using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using VirtualBroker.Property.Business.Core;
using VirtualBroker.Property.Core;

namespace VirtualBroker.Property.ViewModels.Admin
{
    public class UsersViewModel : ManageEntityViewModel<Guid, User, IUserBusinessFacade>
    {
        public UsersViewModel(IUserBusinessFacade facade, ILogger<ManageEntityViewModel<Guid, User, IUserBusinessFacade>> logger) : base(facade, logger)
        {
        }
    }
    public class SelectUsersViewModel : SelectEntitiesViewModel<Guid, User, UserViewModel, IUserBusinessFacade>
    {
        public SelectUsersViewModel(IUserBusinessFacade facade, ILogger<SelectEntitiesViewModel<Guid, User, UserViewModel, IUserBusinessFacade>> logger) : base(facade, logger)
        {
        }

        protected override async Task<UserViewModel> ConstructViewModel(User entity)
        {
            var vm = new UserViewModel(Logger, Facade, entity);
            await vm.Load.Execute().GetAwaiter();
            return vm;
        }
    }
    public class UserLoaderViewModel : EntityLoaderViewModel<Guid, User, UserViewModel, IUserBusinessFacade>
    {
        public UserLoaderViewModel(IUserBusinessFacade facade, ILogger<EntityLoaderViewModel<Guid, User, UserViewModel, IUserBusinessFacade>> logger) : base(facade, logger)
        {
        }

        protected override UserViewModel Construct(User entity)
        {
            return new UserViewModel(Logger, Facade, entity.Id);
        }
    }
    public class UserViewModel : EntityViewModel<Guid, User, IUserBusinessFacade>, IUser
    {
        public UserViewModel(ILogger logger, IUserBusinessFacade facade, Guid id) : base(logger, facade, id)
        {
        }

        public UserViewModel(ILogger logger, IUserBusinessFacade facade, User entity) : base(logger, facade, entity)
        {
        }

        private string objectId = null!;
        public string ObjectId
        {
            get => objectId;
            set => this.RaiseAndSetIfChanged(ref objectId, value);
        }
        private string? firstName;
        public string? FirstName
        {
            get => firstName;
            set => this.RaiseAndSetIfChanged(ref firstName, value);
        }

        private string? lastName;
        public string? LastName
        {
            get => lastName;
            set => this.RaiseAndSetIfChanged(ref lastName, value);
        }

        private string? emailAddress;
        public string? EmailAddress
        {
            get => emailAddress;
            set => this.RaiseAndSetIfChanged(ref emailAddress, value);
        }

       

        protected override Task<User> Populate()
        {
            User user = new User();
            user.Id = Id;
            user.ObjectId = ObjectId;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.EmailAddress = EmailAddress;
            return Task.FromResult(user);
        }

        protected override async Task Read(User entity)
        {
            Id = entity.Id;
            ObjectId = entity.ObjectId;
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            EmailAddress = entity.EmailAddress;
        }
    }
    public class UserBarLoaderViewModel : UserProfileLoaderViewModel
    {
        public UserBarLoaderViewModel(IUserBusinessFacade facade, ILogger<UserProfileLoaderViewModel> logger) : base(facade, logger)
        {
        }
        protected async override Task DoLoad(CancellationToken token)
        {
            try
            {
                var userId = await Facade.GetCurrentUserId(fetchTrueUserId: true, token: token);
                if (userId != null)
                {
                    var user = await Facade.GetByID(userId.Value, token: token);
                    if (user != null)
                    {
                        ViewModel = new UserProfileViewModel(Logger, Facade, user);
                        await ViewModel.Load.Execute().GetAwaiter();
                    }
                }
                else
                    ViewModel = null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                await Alert.Handle(ex.Message).GetAwaiter();
            }
        }
    }
    public class UserProfileLoaderViewModel : ReactiveObject
    {
        public Interaction<string, bool> Alert { get; } = new Interaction<string, bool>();
        public ReactiveCommand<Unit, Unit> Load { get; }
        protected IUserBusinessFacade Facade { get; }
        protected ILogger Logger { get; }
        private UserProfileViewModel? viewModel;
        public UserProfileViewModel? ViewModel
        {
            get => viewModel;
            set => this.RaiseAndSetIfChanged(ref viewModel, value);
        }
        public UserProfileLoaderViewModel(IUserBusinessFacade facade, ILogger<UserProfileLoaderViewModel> logger)
        {
            Facade = facade;
            Logger = logger;
            Load = ReactiveCommand.CreateFromTask(DoLoad);
        }
        protected virtual async Task DoLoad(CancellationToken token)
        {
            try
            {
                var userId = await Facade.GetCurrentUserId(token: token);
                if (userId != null)
                {
                    var user = await Facade.GetByID(userId.Value, token: token);
                    if (user != null)
                    {
                        ViewModel = new UserProfileViewModel(Logger, Facade, user);
                        await ViewModel.Load.Execute().GetAwaiter();
                    }
                }
                else
                    ViewModel = null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                await Alert.Handle(ex.Message).GetAwaiter();
            }
        }
    }
    public class UserProfileViewModel : EntityViewModel<Guid, User>, IUser
    {
        public UserProfileViewModel(ILogger logger, IBusinessRepositoryFacade<User, Guid> facade, Guid id) : base(logger, facade, id)
        {
        }

        public UserProfileViewModel(ILogger logger, IBusinessRepositoryFacade<User, Guid> facade, User entity) : base(logger, facade, entity)
        {
        }

        private string objectId = string.Empty;
        public string ObjectId
        {
            get => objectId;
            set => this.RaiseAndSetIfChanged(ref objectId, value);
        }

        private string? firstName;
        public string? FirstName
        {
            get => firstName;
            set => this.RaiseAndSetIfChanged(ref firstName, value);
        }

        private string? lastName;
        public string? LastName
        {
            get => lastName;
            set => this.RaiseAndSetIfChanged(ref lastName, value);
        }

        private string? emailAddress;
        public string? EmailAddress
        {
            get => emailAddress;
            set => this.RaiseAndSetIfChanged(ref emailAddress, value);
        }

        protected override Task<User> Populate()
        {
            return Task.FromResult(new User()
            {
                Id = Id,
                ObjectId = ObjectId,
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = EmailAddress
            });
        }

        protected async override Task Read(User entity)
        {
            Id = entity.Id;
            ObjectId = entity.ObjectId;
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            EmailAddress = entity.EmailAddress;
        }
    }
}
