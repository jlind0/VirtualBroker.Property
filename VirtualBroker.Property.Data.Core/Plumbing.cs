﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VirtualBroker.Property.Core;

namespace VirtualBroker.Property.Data.Core
{
    /// <summary>
    /// Create DbContext instances and manage user impersonation.
    /// </summary>
    public interface IContextFactory
    {
        DbContext CreateContext();
        Task<ClaimsPrincipal?> GetPrincipal();
        Task<Guid?> GetImpersonatedUser();
        Task SetImpersonatedUser(Guid? userId);
    }
    /// <summary>
    /// Extract user ID.
    /// </summary>
    public static class ClaimsPrincipalExtentension
    {
        public static string? GetUserId(this ClaimsPrincipal? principal)
        {
            return principal?.Claims.FirstOrDefault(p => p.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        }
    }
    /// <summary>
    /// Manage transactions and database context.
    /// </summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        DbContext Context { get; }
        Task Rollback();
        Task Commit(CancellationToken token = default);
    }
    public interface IRepository
    {
        IUnitOfWork CreateUnitOfWork();
        Task<Guid?> GetCurrentUserId(IUnitOfWork? uow = null, bool fetchTrueUserId = false, CancellationToken token = default);
    }
    /// <summary>
    /// Generic repository interface for CRUD operations on entities.
    /// </summary>
    public interface IIRepository<in TEntity, TKey> : IRepository
        where TEntity : Entity<TKey>, new()
        where TKey : struct
    {
        Task Delete(TKey id, IUnitOfWork? work = null, CancellationToken token = default);
        Task Delete(TEntity entity, IUnitOfWork? work = null, CancellationToken token = default);
        Task Add(TEntity entity, IUnitOfWork? work = null, CancellationToken token = default);
    }
    /// <summary>
    /// Extend repository for more querying and update.
    /// </summary>
    public interface IRepository<TEntity, TKey> : IIRepository<TEntity, TKey>
        where TEntity : Entity<TKey>, new()
        where TKey : struct
    {
        Task<TEntity> Update(TEntity entity,
            IUnitOfWork? work = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? properites = null, CancellationToken token = default);
        Task<RepositoryResultSet<TKey, TEntity>> Get(IUnitOfWork? work = null,
            Pager? page = null,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? properites = null,
            CancellationToken token = default);
        Task<int> Count(IUnitOfWork? work = null,
            Expression<Func<TEntity, bool>>? filter = null,
            CancellationToken token = default);
        Task<TEntity?> GetByID(TKey key, IUnitOfWork? work = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? properites = null, CancellationToken token = default);

    }
}
