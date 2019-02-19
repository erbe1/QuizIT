using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        // Defaultkonstruktor krävs
        public ApplicationRole()
        {

        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
