namespace Base.API.DTOs
{
    public class CreateUserDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Department { get; set; }

        public string Password { get; set; } = null!;

    }
}
