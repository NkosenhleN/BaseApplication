using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Responses
{
    public class RoleResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

}
