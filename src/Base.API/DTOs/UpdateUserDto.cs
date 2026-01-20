namespace Base.API.DTOs
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public bool IsActive { get; set; }

        public string Email { get; set; } = null!;
    }
}
