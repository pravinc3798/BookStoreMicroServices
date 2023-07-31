using BookStore.Admin.Entity;
using BookStore.Admin.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdmin _admin;

    public AdminController(IAdmin admin)
    {
        _admin = admin;
    }

    [HttpPost]
    [Route("RegisterAdmin")]
    public IActionResult Register(AdminEntity newAdmin)
    {
        AdminEntity admin = _admin.RegisterAdmin(newAdmin);

        if (admin != null)
        {
            return Ok(new { data = admin, isSuccess = true, message = "admin registered" });
        }
        else
            return BadRequest(new { isSuccess = false, message = "Something went wrong" });
    }

    [HttpPost]
    [Route("AdminLogin")]
    public IActionResult Login(string email, string password)
    {
        string result = _admin.Login(email, password);

        if (result != null)
        {
            return Ok(new { data = result, isSuccess = true, message = "Login Successful" });
        }
        else
            return BadRequest(new { isSuccess = false, message = "Something went wrong" });
    }
}
