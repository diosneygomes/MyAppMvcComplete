using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repositories
{
    public class ProviderRepository : Repository<Provider>, IProviderRepository
    {
        public ProviderRepository(MyDbContext context) : base(context) { }
        public async Task<Provider> GetAddressProviderAsync(Guid id)
        {
            return await Db.Providers.AsNoTracking().Include(p => p.Address)
                    .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Provider> GetProductsAddressProviderAsync(Guid id)
        {
            return await Db.Providers.AsNoTracking()
                .Include(p => p.Products)
                .Include(p => p.Address)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
