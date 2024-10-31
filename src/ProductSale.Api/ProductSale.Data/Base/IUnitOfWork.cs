
using ProductSale.Data.Models;

namespace ProductSale.Data.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Cart> CartRepository { get; }
        IGenericRepository<CartItem> CartItemRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<ChatMessage> ChatMessageRepository { get; }
        IGenericRepository<Notification> NotificationRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<Payment> PaymentRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<StoreLocation> StoreLocationRepository { get; }
        IGenericRepository<User> UserRepository { get; }

        void Save();
    }
}
