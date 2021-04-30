using System;

namespace ServicioAPI.Data.Entities
{
    public class Message : IEntity
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public User Receiver { get; set; }
        public string Text { get; set; }
        public Image Image { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? EditedTime { get; set; }
    }
}
