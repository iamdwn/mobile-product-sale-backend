using System;
using System.Collections.Generic;

namespace ProductSale.Api.Models;

public partial class Chatmessage
{
    public int ChatMessageId { get; set; }

    public int? UserId { get; set; }

    public string? Message { get; set; }

    public DateTime SentAt { get; set; }

    public virtual User? User { get; set; }
}
