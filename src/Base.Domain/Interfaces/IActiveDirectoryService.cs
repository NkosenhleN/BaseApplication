using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    public interface IActiveDirectoryService
    {
        Task<AdUser?> GetUserByEmailAsync(string email);
    }

    public class AdUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }
}
