namespace ProductSale.Data.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }

    public void UpdateTotalPrice()
    {
        TotalPrice = 0;
        foreach (var item in CartItems)
        {
            TotalPrice += item.Price * item.Quantity;
        }
    }
}
