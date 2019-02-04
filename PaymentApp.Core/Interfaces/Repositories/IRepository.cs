using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentApp.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        Task<T> CreateAsync(T entity);
        Task<T> DeleteAsync(int id);
        Task<IEnumerable<T>> FindAsync(Func<T, bool> expression = null);
        Task<T> GetByIdAsync(int id);
        Task<T> SoftDeleteAsync(int id);
        Task<int> UpdateChangesAsync(T updatedEntity = null);
    }
}
