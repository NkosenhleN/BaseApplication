using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }

        public string UserName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public byte[] PasswordHash { get; private set; } = null!;
        public byte[] PasswordSalt { get; private set; } = null!;
        public bool IsActive { get; private set; }
        public bool IsLocked { get; private set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTime? LastLoginAt { get; private set; }
        public DateTime? PasswordChangedAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private User() { } 

        public User(
            string username,
            string firstName,
            string lastName,
            string email,
            byte[] passwordHash,
            byte[] passwordSalt)
        {
            Id = Guid.NewGuid();
            UserName = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;

            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;

            IsActive = true;
            IsLocked = false;
            FailedLoginAttempts = 0;

            CreatedAt = DateTime.UtcNow;
        }

        public void RecordLoginSuccess()
        {
            FailedLoginAttempts = 0;
            LastLoginAt = DateTime.UtcNow;
        }

        public void RecordLoginFailure()
        {
            FailedLoginAttempts++;

            if (FailedLoginAttempts >= 5)
            {
                IsLocked = true;
            }
        }

        public void ChangePassword(byte[] newHash, byte[] newSalt)
        {
            PasswordHash = newHash;
            PasswordSalt = newSalt;
            PasswordChangedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }

}
