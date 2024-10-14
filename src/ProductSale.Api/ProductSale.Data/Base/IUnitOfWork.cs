

using ProductSale.Api.Models;

namespace ProductSale.Data.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Cart> CartRepository { get; }
        IGenericRepository<Cartitem> CartitemRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<Chatmessage> ChatmessageRepository { get; }
        IGenericRepository<Notification> NotificationRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<Payment> PaymentRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<Storelocation> StorelocationRepository { get; }
        IGenericRepository<User> UserRepository { get; }

        void Save();
    }
}
