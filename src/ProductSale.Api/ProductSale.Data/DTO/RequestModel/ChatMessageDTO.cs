namespace ProductSale.Data.DTO.RequestModel
{
    public class ChatMessageDTO
    {
        public int RoomId { get; set; }

        public int? UserId { get; set; }

        public string? Message { get; set; }

        public DateTime SentAt { get; set; }
    }
}
