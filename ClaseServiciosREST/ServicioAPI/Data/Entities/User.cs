namespace ServicioAPI.Data.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public Image Image { get; set; }
        public string Description { get; set; }
        public string UrlPage { get; set; }
    }
}
