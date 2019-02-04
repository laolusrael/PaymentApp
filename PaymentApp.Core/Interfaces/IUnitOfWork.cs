using PaymentApp.Core.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace PaymentApp.Core.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        Task SaveChangesAsync();
        
        IOrderRepository OrderRepository { get; set; }
        ICustomerRepository CustomerRepository { get; set; }
        IOrderItemRepository OrderItemRepository { get; set; }

        //Transaction Management
        void StartTransaction();
        void RollbackTransaction();
        void CommitTransaction();
    }
}
