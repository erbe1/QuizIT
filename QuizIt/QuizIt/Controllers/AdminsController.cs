using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizIt.Data;
using QuizIt.Models.ViewModels;
using QuizIt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Controllers
{
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _auth;

        public AdminsController(ApplicationDbContext context, AuthService auth)
        {
            _context = context;
            _auth = auth;
        }

        public IActionResult Migrate()
        {
            _context.Database.Migrate();
            return Ok("Migrate done");
        }
        //[Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var vm = new AddRoleVm
            {
                AllRoles = _context.Roles.Select(role => new SelectListItem() { Text = role.Name, Value = role.Name }),
                AllUsers = _context.Users.Select(user => new SelectListItem() { Text = user.Email, Value = user.Email })
            };
            return View(vm);
        }

        public async Task<IActionResult> AddRoleForUser(AddRoleVm addrole)
        {

            await _auth.AddToRoleAsync(addrole.Email, addrole.Role);
            return View("SuccessAddRole", addrole);

        }
    }
}
