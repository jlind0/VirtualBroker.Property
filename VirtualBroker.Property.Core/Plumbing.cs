﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualBroker.Property.Core
{
    public interface IEntity<TKey>
        where TKey : struct
    {
        TKey Id { get; set; }
    }
    public interface IUserPartionedEntity
    {
        Guid UserId { get; set; }
    }
    public interface INamedEntity
    {
        string Name { get; set; }
    }
    public interface ICodedEntity
    {
        string Code { get; set; }
    }
    public abstract class Entity<TKey> : IEntity<TKey>
        where TKey : struct
    {
        public TKey Id { get; set; }
        public Guid? CreatedByUserId { get; set; }

        public Guid? UpdatedByUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual User? CreatedByUser { get; set; }

        public virtual User? UpdatedByUser { get; set; }
    }
    public class RepositoryResultSet<TKey, TEntity> : IPagedResult<TEntity>
        where TEntity : Entity<TKey>, new()
        where TKey : struct
    {
        public IEnumerable<TEntity> Entities { get; set; } = null!;
        public int? Count { get; set; }
        public int? PageSize { get; set; }
        public int? Page { get; set; }


    }
    public interface IPagedResult<T>
    {
        IEnumerable<T> Entities { get; set; }
        int? Count { get; set; }
        int? PageSize { get; set; }
        int? Page { get; set; }
    }
    public struct Pager
    {
        public int Size { get; set; }
        public int Page { get; set; }
    }
}
