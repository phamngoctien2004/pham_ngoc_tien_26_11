using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateOnly Birth { get; set; }
        public string Password { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role Role { get; set; } = new Role();
    }
}
