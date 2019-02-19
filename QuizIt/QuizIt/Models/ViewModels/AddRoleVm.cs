using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Models.ViewModels
{
    public class AddRoleVm
    {
        public IEnumerable<SelectListItem> AllRoles { get; set; }

        public IEnumerable<SelectListItem> AllUsers { get; set; }

        [Required(ErrorMessage = "Du måste ange en email.")]
        [EmailAddress(ErrorMessage = "Du har ej angett en korrekt email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Du måste ange en roll.")]
        public string Role { get; set; }
    }
}
