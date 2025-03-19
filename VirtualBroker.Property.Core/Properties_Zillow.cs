using System;
using System.Collections.Generic;

namespace VirtualBroker.Property.Core;

public interface IProperties_Zillow : IEntity<Guid>
{
    Guid ApiId { get; set; }
    string AffiliatedLink { get; set; }
    string Json { get; set; }
    string Zpid { get; set; }
}
public partial class Properties_Zillow : Entity<Guid>, IProperties_Zillow
{
    public Guid ApiId { get; set; }

    public string AffiliatedLink { get; set; } = null!;

    public string Json { get; set; } = null!;

    public string Zpid { get; set; } = null!;

    public virtual APIRequests_Zillow? Api { get; set; }
}
