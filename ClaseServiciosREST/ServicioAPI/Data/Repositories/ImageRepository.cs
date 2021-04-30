using ServicioAPI.Data.Entities;

namespace ServicioAPI.Data.Repositories
{
    public class ImageRepository : GenericRepository<Image>
    {
        public ImageRepository(DataBase context) : base(context)
        {
        }
    }
}
