using Base.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Entities
{
    public class Role : AuditableEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public ICollection<User> Users { get; private set; } = new List<User>();


        private Role() { }

        public Role(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            CreatedAt = DateTime.UtcNow;
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Role name cannot be empty");

            Name = newName.Trim();
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            if (IsDeleted) return;
            MarkDeleted();
        }

    }

}
