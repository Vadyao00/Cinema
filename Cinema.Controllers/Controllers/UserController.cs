using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Contracts.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Controllers.Controllers
{
    public class UserController : ApiControllerBase
    {
        private readonly IServiceManager _service;
        private readonly UserManager<User> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;

        public UserController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IServiceManager serviceManager)
        {
                _roleManager = roleManager;
                _userManager = userManager;
                _service = serviceManager;
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

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst("sub")?.Value;
            if (userId == null)
            {
                return Unauthorized("Не удалось определить пользователя.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            return Ok(new { Message = "Пароль успешно изменён." });
        }
    }

    public class ChangePasswordRequest
    {
        [Required]
        public string? OldPassword { get; set; }

        [Required]
        [MinLength(6)]
        public string? NewPassword { get; set; }
    }
}