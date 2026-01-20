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

        private Role() { }

        public Role(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            CreatedAt = DateTime.UtcNow;
        }



    }

}
