using DevIO.Business.Models;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProviderRepository : IRepository<Provider>
    {
        Task<Provider> GetAddressProviderAsync(Guid id);
        Task<Provider> GetProductsAddressProviderAsync(Guid id);
    }
}
