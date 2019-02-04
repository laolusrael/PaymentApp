using PaymentApp.Core.Interfaces;
using System;
using System.Threading.Tasks;
using System.Transactions;
using PaymentApp.Core.Interfaces.Repositories;

namespace PaymentApp.Persistence
{
    public class UnitOfWork<TContext> : IUnitOfWork  where TContext:PaymentAppContext
    {



        #region Construction

        private readonly TContext _context;
        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        #endregion


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();  
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion


        public IOrderRepository OrderRepository { get; set; }
        public ICustomerRepository CustomerRepository { get; set; }
        public IOrderItemRepository OrderItemRepository { get; set; }

        public async Task SaveChangesAsync()
        {
            // Perform your audits
            // handle transactions
            // commit changes

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #region Transaction Management

        private TransactionScope _workTransaction;

        public void StartTransaction()
        {

            if (_workTransaction == null)
                _workTransaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        public void RollbackTransaction()
        {
            if(_workTransaction != null)
            {
                _workTransaction.Dispose(); // Rollback();
            }
        }

        public void CommitTransaction()
        {
            if(_workTransaction != null)
            {
                _workTransaction.Complete();
                // Transactions are completed.
                _workTransaction.Dispose();
            }
        }

        #endregion
    }
}
