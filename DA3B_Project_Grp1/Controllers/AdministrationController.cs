using DA3B_Project_Grp1.Models;
using DA3B_Project_Grp1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DA3B_Project_Grp1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<MyIdentityRole> _roleManager;

        //UserManager class is used to manage users e.g. registering new users,
        //validating credentials and loading user information

        public UserManager<MyIdentityUser> _UserManager { get; }

        public AdministrationController(RoleManager<MyIdentityRole> roleManager,
                                           UserManager<MyIdentityUser> userManager)
        {
            //I don't have to prefix fields with this because setting called
            //dotnet_style_qualification_for_field is by default set to false
            
            _roleManager = roleManager;
            _UserManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole() 
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model) 
        {
            if (ModelState.IsValid)
            {
                MyIdentityRole identityRole = new MyIdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            
            return View(model);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            //Roles property list all role objects
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("Not found");
            }

            var model = new EditRoleViewModel
            {
                //Setting the Id property of RoleViewmodel object with id property of above role object
                Id = role.Id,
                RoleName = role.Name
            };
            //Will going to traverse through all the users
            foreach(var user in _UserManager.Users)
            {
                //Checking if user belongs to the specified role name
                if(await _UserManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("Not found");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
           
        }
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();

            foreach(var user in _UserManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id.ToString(),
                    UserName = user.UserName
                };

                if(await _UserManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            //Let's try to retireve the respected Role from the database using RoleId
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role==null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            for(int i=0; i < model.Count; i++)
            {
                var user = await _UserManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;
                if (model[i].IsSelected  && !(await _UserManager.IsInRoleAsync(user, role.Name)))    //This means the user is selected inside UI and we want to add users in table
                {
                    result = await _UserManager.AddToRoleAsync(user, role.Name);
                }
                else if(!model[i].IsSelected && await _UserManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _UserManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            //if model parameter is emoty it means we do not have any userrole objects
            return RedirectToAction("EditRole", new { Id = role.Id });
        }

    }
}
