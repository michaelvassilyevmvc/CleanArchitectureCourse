using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Interefaces 
{
    public interface IDbContext
    {
        public DbSet<Order> Orders { get; }

        public DbSet<Product> Products { get; }

        Task<int> SaveChangesAsync(CancellationToken token = default);
    }
}
