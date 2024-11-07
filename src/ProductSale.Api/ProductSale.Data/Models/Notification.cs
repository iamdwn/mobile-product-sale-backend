using AutoMapper.Configuration.Annotations;
using System.Text.Json.Serialization;

namespace ProductSale.Data.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? UserId { get; set; }

    public string? Message { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }
    [JsonIgnore]
    public virtual User? User { get; set; }
}
