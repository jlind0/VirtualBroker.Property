using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VirtualBroker.Property.Core;
using VirtualBroker.Property.Data.Core;

namespace VirtualBroker.Property.Data
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(IContextFactory contextFactory, IMemoryCache cache) : base(contextFactory, cache)
        {
        }
        
        public async Task<User?> GetByObjectIdAsync(string objectId, IUnitOfWork? work = null, Expression<Func<User, object>>? properties = null, CancellationToken token = default)
        {
            User? user = null;
            await Use(async (w, t) =>
            {
                var query = w.VirtualBrokersContext.Users.AsQueryable();
                if (properties != null)
                    query = query.Include(properties);
                user = await query.SingleOrDefaultAsync(x => x.ObjectId == objectId);
            }, work, token);
            return user;
        }
    }
    public class RoleRepository : Repository<Role, Guid>, IRoleRepository
    {
        public RoleRepository(IContextFactory contextFactory, IMemoryCache cache) : base(contextFactory, cache)
        {
        }

        public async Task<Guid[]> GetUserIds(Guid roleId, IUnitOfWork? work = null, CancellationToken token = default)
        {
            Guid[] userIds = [];
            await Use(async (w, t) =>
            {
                userIds = await w.VirtualBrokersContext.Users.Where(u => u.Roles.Any(r => r.Id == roleId)).Select(u => u.Id).ToArrayAsync(token);
            }, work, token);
            return userIds;
        }

        public async Task SetSelectedUsersToRole(Guid roleId, Guid[] userIds, IUnitOfWork? work = null, CancellationToken token = default)
        {
            await Use(async (w, t) =>
            {
                var role = await w.VirtualBrokersContext.Roles.Include(c => c.Users).SingleOrDefaultAsync(w => w.Id == roleId);
                if (role != null)
                {

                    var userId = await GetCurrentUserId(w, true, token: t);
                    role.Users.Clear();
                    role.UpdateDate = DateTime.UtcNow;
                    role.UpdatedByUserId = userId;
                    foreach (var id in userIds)
                    {
                        role.Users.Add(await w.VirtualBrokersContext.Users.SingleAsync(w => w.Id == id));
                    }

                }
            }, work, token, true);
        }
    }
}
