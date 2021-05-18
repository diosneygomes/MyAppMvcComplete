using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DevIO.Data.Context;

namespace DevIO.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MyDbContext context) : base(context){}
        public async Task<Product> GetProviderProductAsync(Guid id)
        {
            return await Db.Products.AsNoTracking().Include(p => p.Provider)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsProvidersAsync()
        {
            return await Db.Products.AsNoTracking().Include(p => p.Provider)
                .OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProvidersProductsAsync(Guid providerId)
        {
            return await SearchAsync(p => p.ProviderId == providerId);
        }
    }
}
