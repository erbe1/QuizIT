using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using QuizIt.Models;

namespace QuizIt.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        internal async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> UserExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<bool> IsInRoleAsync(string email, string rolename)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return await _userManager.IsInRoleAsync(user, rolename);
        }

        public async Task<bool> RoleExistsAsync(string rolename)
        {
            return await _roleManager.RoleExistsAsync(rolename);
        }

        public async Task CreateRoleAsync(string rolename)
        {
            await _roleManager.CreateAsync(new ApplicationRole(rolename));
        }

        public async Task AddToRoleAsync(string email, string rolename)
        {
            var user = await _userManager.FindByEmailAsync(email);
            await _userManager.AddToRoleAsync(user, rolename);
        }

        public async Task CreateAdminIfNotExist()
        {
            // todo: gör nåt med denna :)
            const string email = "admin@quizit.com";
            const string role = "Admin";
            const string password = "aQ!234567890";

            // Skapa användare

            ApplicationUser user = await _userManager.FindByNameAsync(email);

            if (user == null)
            {
                IdentityResult result = await _userManager.CreateAsync(new ApplicationUser(email), password);
                if (!result.Succeeded)
                    throw new Exception("Couldn't create superadmin");
            }

            // Skapa roll

            bool roleExist = await RoleExistsAsync(role); // _roleManager.RoleExistsAsync(role);

            if (!roleExist)
            {
                IdentityResult result = await _roleManager.CreateAsync(new ApplicationRole(role));
                if (!result.Succeeded)
                    throw new Exception("Couldn't create role");
            }

            // Sätt användare till rollen

            if (!await _userManager.IsInRoleAsync(user, role))
            {
                IdentityResult result = await _userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                    throw new Exception("Couldn't set user for role");
            }

        }
    }
}
