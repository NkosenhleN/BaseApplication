using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public DateTime? DeletedAt { get; protected set; }

        public bool IsDeleted => DeletedAt.HasValue;

        protected void MarkUpdated() => UpdatedAt = DateTime.UtcNow;
        protected void MarkDeleted() => DeletedAt = DateTime.UtcNow;
    }

}
