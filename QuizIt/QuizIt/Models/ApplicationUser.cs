using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Defaultkonstruktor krävs
        public ApplicationUser()
        {

        }

        public ApplicationUser(string userName) : base(userName)
        {
        }

        // dina extra properties. Blir en ny kolumn i "AspNetUsers"
        //public List<Quiz> SavedQuizzes { get; set; }
    }
}
