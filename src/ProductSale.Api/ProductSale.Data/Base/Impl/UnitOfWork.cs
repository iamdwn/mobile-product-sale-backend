﻿using ProductSale.Api.Models;
using ProductSale.Data.Persistences;
using System.ComponentModel.DataAnnotations;

namespace ProductSale.Data.Base
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ProductSaleContext context;
        private GenericRepository<Cart> cartRepository;
        private GenericRepository<Cartitem> cartItemRepository;
        private GenericRepository<Category> categoryRepository;
        private GenericRepository<Chatmessage> chatMessageRepository;
        private GenericRepository<Notification> notificationRepository;
        private GenericRepository<Order> orderRepository;
        private GenericRepository<Payment> paymentRepository;
        private GenericRepository<Product> productRepository;
        private GenericRepository<Storelocation> storelocationRepository;
        private GenericRepository<User> userRepository;

        public UnitOfWork(ProductSaleContext _context)
        {
            context = _context;
        }

        public IGenericRepository<Cart> CartRepository
        {
            get
            {
                return cartRepository ??= new GenericRepository<Cart>(context);
            }
        }

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                return categoryRepository ??= new GenericRepository<Category>(context);
            }
        }

        public IGenericRepository<Cartitem> CartitemRepository
        {
            get
            {
                return cartItemRepository ??= new GenericRepository<Cartitem>(context);
            }
        }

        public IGenericRepository<Chatmessage> ChatmessageRepository
        {
            get
            {
                return chatMessageRepository ??= new GenericRepository<Chatmessage>(context);
            }
        }

        public IGenericRepository<Order> OrderRepository
        {
            get
            {
                return orderRepository ??= new GenericRepository<Order>(context);
            }
        }

        public IGenericRepository<Notification> NotificationRepository
        {
            get
            {
                return notificationRepository ??= new GenericRepository<Notification>(context);
            }
        }

        public IGenericRepository<Payment> PaymentRepository
        {
            get
            {
                return paymentRepository ??= new GenericRepository<Payment>(context);
            }
        }

        public IGenericRepository<Storelocation> StorelocationRepository
        {
            get
            {
                return storelocationRepository ??= new GenericRepository<Storelocation>(context);
            }
        }

        public IGenericRepository<Product> ProductRepository
        {
            get
            {
                return productRepository ??= new GenericRepository<Product>(context);
            }
        }

        public IGenericRepository<User> UserRepository
        {
            get
            {
                return userRepository ??= new GenericRepository<User>(context);
            }
        }

        public void Save()
        {
            var validationErrors = context.ChangeTracker.Entries<IValidatableObject>()
                .SelectMany(e => e.Entity.Validate(null))
                .Where(e => e != ValidationResult.Success)
                .ToArray();
            if (validationErrors.Any())
            {
                var exceptionMessage = string.Join(Environment.NewLine,
                    validationErrors.Select(error => $"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
                throw new Exception(exceptionMessage);
            }
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
