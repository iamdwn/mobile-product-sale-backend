using ProductSale.Data.Models;

namespace ProductSale.Data.DTO.ResponseModel
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = null!;
        public virtual ICollection<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
    }

    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
