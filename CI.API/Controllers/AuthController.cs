using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CI.API.ViewModels;
using CI.DAL;
using CI.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CI.API.Controllers
{
  [ApiController]
  [Route("api/{controller}")]
  public class AuthController : ControllerBase
  {
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

     public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    } 

    // Post api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
          return BadRequest();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        } 
        return Ok(result);
    }
  }
}