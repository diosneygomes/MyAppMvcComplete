using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface  IProviderService : IDisposable
    {
        Task AddAsync(Provider provider);
        Task UpdateAsync(Provider provider);
        Task RemoveAsync(Guid id);
        Task AddressUpdateAsync(Address address);
    }
}
