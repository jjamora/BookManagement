using BM.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace BM.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
