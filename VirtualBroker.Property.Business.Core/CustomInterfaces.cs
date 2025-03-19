using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VirtualBroker.Property.Core;
using VirtualBroker.Property.Data.Core;

namespace VirtualBroker.Property.Business.Core
{
    public interface IUserBusinessFacade : IBusinessRepositoryFacade<User, Guid>
    {
        /// <summary>
        /// Get user by Azure AD B2C object id
        /// </summary>
        /// <param name="objectId">Target Azure AD B2C Object Id</param>
        /// <param name="work">Optional unit of work</param>
        /// <param name="properties">Optional User Properites to load</param>
        /// <param name="token">Optional cancellation token</param>
        /// <returns>User, if found</returns>
        Task<User?> GetByObjectIdAsync(string objectId, IUnitOfWork? work = null, Expression<Func<User, object>>? properties = null, CancellationToken token = default);
    }
    /// <summary>
    /// Role Business Facade
    /// </summary>
    public interface IRoleBusinessFacade : IBusinessRepositoryFacade<Role, Guid>
    {
        /// <summary>
        /// Get User Ids for a Role
        /// </summary>
        /// <param name="roleId">Target Role.Id</param>
        /// <param name="work">Optional Unit of Work</param>
        /// <param name="token">Optional Cancellation Token</param>
        /// <returns>Array of User.Id's</returns>
        Task<Guid[]> GetUserIds(Guid roleId, IUnitOfWork? work = null, CancellationToken token = default);
        /// <summary>
        /// Set selected users to a role
        /// </summary>
        /// <param name="roleId">Target Role.Id</param>
        /// <param name="userIds">Target User.Id's</param>
        /// <param name="work">Optional Unit of Work</param>
        /// <param name="token">Optional Cancellation Token</param>
        /// <returns>Task represent work of assigning roles</returns>
        Task SetSelectedUsersToRole(Guid roleId, Guid[] userIds, IUnitOfWork? work = null, CancellationToken token = default);
    }
}
