using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsProvidersAsync();
        Task<IEnumerable<Product>> GetProvidersProductsAsync(Guid providerId);
        Task<Product> GetProviderProductAsync(Guid id);
    }
}
