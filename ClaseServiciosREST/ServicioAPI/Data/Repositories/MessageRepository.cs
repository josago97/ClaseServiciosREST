using Microsoft.EntityFrameworkCore;
using ServicioAPI.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ServicioAPI.Data.Repositories
{
    public class MessageRepository : GenericRepository<Message>
    {
        public MessageRepository(DataBase context) : base(context)
        {
        }

        public override async Task<Message> GetByIdAsync(int id)
        {
            return await context.Messages.Include(m => m.Image).Include(m => m.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public Message[] GetMessagesByReceiver(User receiver)
        {
            return context.Messages.Include(m => m.Image).Include(m => m.Author)
                .Where(m => m.Receiver.Id == receiver.Id).ToArray();
        }

        public Message[] GetMessagesByAuthor(User author)
        {
            return context.Messages.Include(m => m.Image).Include(m => m.Author)
                .Where(m => m.Author.Id == author.Id).ToArray();
        }
    }
}
