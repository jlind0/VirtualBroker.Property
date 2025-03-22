using System;
using System.Collections.Generic;

namespace VirtualBroker.Property.Core;

public interface IAPIRequests_Zillow : IEntity<Guid>, INamedEntity, ICodedEntity
{
    string ApiKey { get; set; }
    string ApiHost { get; set; }
    string RequestUri { get; set; }
}
public partial class APIRequests_Zillow : Entity<Guid>, IAPIRequests_Zillow
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string ApiKey { get; set; } = null!;

    public string ApiHost { get; set; } = null!;

    public string RequestUri { get; set; } = null!;


    public virtual ICollection<Properties_Zillow> Properties_Zillows { get; set; } = [];
}
