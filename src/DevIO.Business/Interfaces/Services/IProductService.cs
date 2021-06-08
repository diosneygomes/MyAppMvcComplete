using DevIO.Business.Models;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProductService : IDisposable
    {
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task RemoveAsync(Guid id);
    }
}
