using System;
using System.Collections.Generic;

namespace VirtualBroker.Property.Core;

public interface IRole : IEntity<Guid>, INamedEntity
{

}
public partial class Role : Entity<Guid>, IRole
{

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = [];
}
