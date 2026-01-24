using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Commands
{
    public record LoginCommand( string UserName, string Password);
}
