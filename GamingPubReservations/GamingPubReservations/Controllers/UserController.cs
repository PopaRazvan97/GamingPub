using BusinessLayer.Dtos;
using BusinessLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingPubReservations.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("all-users")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUserById([FromBody] IdDto user)
        {
            if (_userService.RemoveUserById(user))
            {
                return Ok("User deleted");
            }
            return BadRequest("User is not in list of users");
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateUser([FromBody] UpdateUserDto user)
        {
            if (_userService.UpdateUser(user))
            {
                return Ok("User updated");
            }
            return BadRequest("User was not updated successfully");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterDto registerData)
        {
            if (_userService.Register(registerData))
            {
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest("Something wrong happened");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDto loginData)
        {
            var jwtToken = _userService.ValidateLogin(loginData);
            return Ok(new { token = jwtToken });
        }
    }
}