using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    public class UserController : ApiControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
                _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.OrderBy(user => user.Id);

            List<UserDto> userDtos = [];

            string urole = "";
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Count > 0)
                {
                    urole = userRoles[0] ?? "";
                }

                userDtos.Add(
                    new UserDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Name = user.FirstName + user.LastName,
                        Email = user.Email,
                        UserRole = urole
                    });
            }

            return Ok(userDtos);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] UserForUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.OldUserName);
                if (user != null)
                {
                    var oldRoles = await _userManager.GetRolesAsync(user);

                    if (oldRoles.Count > 0)
                    {
                        await _userManager.RemoveFromRolesAsync(user, oldRoles);

                    }
                    var newRole = model.UserRole;
                    if (newRole.Length > 0)
                    {
                        await _userManager.AddToRoleAsync(user, newRole);
                    }
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.FirstName = model.FirstName;
                    user.LastName = model.SecondName;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return Ok();
        }

        [HttpDelete("delete/{name}")]
        public async Task<ActionResult> Delete(string name)
        {
            User user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return Ok();
        }
    }
}