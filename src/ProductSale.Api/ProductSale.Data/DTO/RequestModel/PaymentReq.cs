namespace ProductSale.Data.DTO.RequestModel
{
    public class PaymentReq
    {
        public int PaymentId { get; set; }

        public int? OrderId { get; set; }

        public decimal Amount { get; set; }

        public string? Note { get; set; }
    }
}
