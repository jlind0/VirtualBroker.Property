using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VirtualBroker.Property.Core;

namespace VirtualBroker.Property.Data.Core
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetByObjectIdAsync(string objectId, IUnitOfWork? work = null, Expression<Func<User, object>>? properties = null, CancellationToken token = default);
    }
    public interface IRoleRepository : IRepository<Role, Guid>
    {
        Task<Guid[]> GetUserIds(Guid roleId, IUnitOfWork? work = null, CancellationToken token = default);
        Task SetSelectedUsersToRole(Guid roleId, Guid[] userIds, IUnitOfWork? work = null, CancellationToken token = default);
    }
}
