using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VirtualBroker.Property.Business.Core;
using VirtualBroker.Property.Core;
using VirtualBroker.Property.Data.Core;

namespace VirtualBroker.Property.Business
{
    public class UserBusinessFacade : BusinessRepositoryFacade<User, Guid, IUserRepository>, IUserBusinessFacade
    {
        public UserBusinessFacade(IUserRepository repository, ILogger<User> logger) : base(repository, logger)
        {
        }

        public Task<User?> GetByObjectIdAsync(string objectId, IUnitOfWork? work = null, Expression<Func<User, object>>? properties = null, CancellationToken token = default)
        {
            return Repository.GetByObjectIdAsync(objectId, work, properties, token);
        }
    }
    public class RoleBusinessFacade : BusinessRepositoryFacade<Role, Guid, IRoleRepository>, IRoleBusinessFacade
    {
        public RoleBusinessFacade(IRoleRepository repository, ILogger<Role> logger) : base(repository, logger)
        {
        }

        public Task<Guid[]> GetUserIds(Guid roleId, IUnitOfWork? work = null, CancellationToken token = default)
        {
            return Repository.GetUserIds(roleId, work, token);
        }

        public Task SetSelectedUsersToRole(Guid roleId, Guid[] userIds, IUnitOfWork? work = null, CancellationToken token = default)
        {
            return Repository.SetSelectedUsersToRole(roleId, userIds, work, token);
        }
    }
}
