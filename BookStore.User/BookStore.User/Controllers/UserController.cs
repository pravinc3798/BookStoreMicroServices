using BookStore.User.Entity;
using BookStore.User.Interface;
using BookStore.User.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(UserEntity newUser)
        {
            UserEntity user = _user.Register(newUser);

            if (user != null)
            {
                return Ok(new { data = user, isSuccess = true, message = "User registered" });
            }
            else
                return BadRequest(new { isSuccess = false, message = "Something went wrong" });
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string email, string password)
        {
            string result = _user.Login(email, password);

            if (result != null)
            {
                return Ok(new { data = result, isSuccess = true, message = "Login Successful" });
            }
            else
                return BadRequest(new { isSuccess = false, message = "Something went wrong" });
        }

        [Authorize]
        [HttpGet]
        [Route(("UserDetails"))]
        public IActionResult GetUserDetails()
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Sid));
            UserEntity user = _user.GetUserDetails(userId);

            if (user != null)
            {
                return Ok(new { data = user, isSuccess = true, message = "User Details" });
            }
            else
                return BadRequest(new { isSuccess = false, message = "Something went wrong" });
        }
    }
}
