using Microsoft.EntityFrameworkCore;
using ServicioAPI.Data.Entities;


namespace ServicioAPI.Data
{
    public class DataBase : DbContext
    {
        public DataBase(DbContextOptions<DataBase> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
