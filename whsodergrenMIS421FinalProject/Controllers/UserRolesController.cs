using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using whsodergrenMIS421FinalProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace whsodergrenMIS421FinalProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();
            var userRoles = new Dictionary<string, string>();

            foreach (var user in users)
            {
                var rolesForUser = _userManager.GetRolesAsync(user).Result;
                userRoles[user.Id] = rolesForUser.Any() ? string.Join(", ", rolesForUser) : "None";
            }

            var viewModel = new UsersAndRoles
            {
                Users = users,
                Roles = roles,
                UserRoles = userRoles
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleList = new SelectList(roles, "Name", "Name", userRoles);

            var viewModel = new UsersAndRoles
            {
                Users = new List<IdentityUser> { user },
                Roles = roles,
                UserRoles = new Dictionary<string, string> { { user.Id, string.Join(", ", userRoles) } },
                RoleList = roleList
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [FromForm] string roleName)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found";
                return View("NotFound");
            }

            // Retrieve current roles and clear them
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return View(user);
            }

            // If a role was selected, add the new role
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var addRoleResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!addRoleResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to add user to the new role");
                    return View(user);
                }
            }

            // Check if the edited user is the current logged-in user and force a logout if roles were changed
            if (User.Identity.Name == user.UserName)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return RedirectToAction("ForcedLogout");  // Redirect to the ForcedLogout action
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    // Handle errors, for example: show an error message or log it
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ForcedLogout()
        {
            return View();
        }
    }
}
