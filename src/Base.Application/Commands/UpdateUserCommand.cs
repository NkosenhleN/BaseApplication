using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Commands
{
    public record UpdateUserCommand(
         string? FirstName,
         string? LastName, 
         string? Email, 
         bool? IsActive
        );
}
