using System;
using System.Collections.Generic;

namespace ProductSale.Api.Models;

public partial class Storelocation
{
    public int LocationId { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string Address { get; set; } = null!;
}
