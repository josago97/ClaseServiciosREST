using Microsoft.EntityFrameworkCore;
using ServicioAPI.Data.Entities;
using System.Threading.Tasks;

namespace ServicioAPI.Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(DataBase context) : base(context)
        {
        }

        public async Task<User> GetUserByNicknameAsync(string nickname)
        {
            return await context.Users
                .Include(u => u.Image)
                .FirstOrDefaultAsync(u => u.Nickname == nickname);
        }

        public async Task<User> GetUserByTokenAsync(string token)
        {
            return await context.Users
                .Include(u => u.Image)
                .FirstOrDefaultAsync(u => u.Token == token);
        }
    }
}
