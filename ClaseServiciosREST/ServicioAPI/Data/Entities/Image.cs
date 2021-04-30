namespace ServicioAPI.Data.Entities
{
    public class Image : IEntity
    {
        public int Id { get; set; }
        public string Format { get; set; }
        public byte[] Data { get; set; }
    }
}
