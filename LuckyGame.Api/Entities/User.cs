namespace LuckyGame.Api.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get;  set; }
        public string Password { get;  set; }
        public string Role { get; set; }
        public int Balance { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
   
    }
}
