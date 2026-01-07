namespace Base.API.DTOs
{
    public class UserReadDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Department { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
