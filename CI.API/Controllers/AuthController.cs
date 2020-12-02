using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
using Microsoft.IdentityModel.Tokens;

namespace CI.API.Controllers
{
  [ApiController]
  [Route("api/{controller}")]
  public class AuthController : ControllerBase
  {
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthController(IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager)
    {
      _config = config;
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
      var token = await JwtTokenGenerator(user);
      return Ok(new { result = result, token = token });
    }

    // Post api/auth/confirmemail
    [HttpPost("confirmemail")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewmodel model)
    {
      var employer = await _userManager.FindByIdAsync(model.UserId);
      var confirm = await _userManager.ConfirmEmailAsync(employer, Uri.UnescapeDataString(model.Token));

      if (confirm.Succeeded)
      {
        return Ok();
      }

      return Unauthorized();
    }

    private async Task<string> JwtTokenGenerator(User userInfo)
    {
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, userInfo.Id),
        new Claim(ClaimTypes.Name, userInfo.UserName)
      };

      var roles = await _userManager.GetRolesAsync(userInfo);
      foreach (var role in roles)
      {
        claims.Add(new Claim(ClaimTypes.Role, role));
      }

      var securityKey = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(_config.GetSection("AppSettings:Key").Value));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = credentials
      };
      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }
  }
}