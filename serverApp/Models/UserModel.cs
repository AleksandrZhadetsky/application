namespace application.Server.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public Guid Password { get; set; }
    }
}