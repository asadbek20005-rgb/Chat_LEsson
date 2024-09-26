namespace Chat.Client.DTOs.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime CreatedDate => DateTime.UtcNow;
        public byte? Age { get; set; }
        public string Role { get; set; }
    }
}
