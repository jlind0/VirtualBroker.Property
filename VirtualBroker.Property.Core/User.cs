using System;
using System.Collections.Generic;

namespace VirtualBroker.Property.Core;

public interface IUser : IEntity<Guid>
{
    string ObjectId { get; set; }
    string? FirstName { get; set; }
    string? LastName { get; set; }
    string? EmailAddress { get; set; }
}
public partial class User : Entity<Guid>, IUser
{

    public string ObjectId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddress { get; set; }

    public virtual ICollection<APIRequests_Zillow> APIRequests_ZillowCreatedByUsers { get; set; } = new List<APIRequests_Zillow>();

    public virtual ICollection<APIRequests_Zillow> APIRequests_ZillowUpdatedByUsers { get; set; } = new List<APIRequests_Zillow>();

    public virtual ICollection<User> InverseCreatedByUser { get; set; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByUser { get; set; } = new List<User>();

    public virtual ICollection<Properties_Zillow> Properties_ZillowCreatedByUsers { get; set; } = new List<Properties_Zillow>();

    public virtual ICollection<Properties_Zillow> Properties_ZillowUpdatedByUsers { get; set; } = new List<Properties_Zillow>();

    public virtual ICollection<Role> RoleCreatedByUsers { get; set; } = new List<Role>();

    public virtual ICollection<Role> RoleUpdatedByUsers { get; set; } = new List<Role>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
