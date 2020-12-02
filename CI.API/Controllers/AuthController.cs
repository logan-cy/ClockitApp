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
using CI.SER.DTOs;
using CI.SER.Interfaces;
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
    public IEmail _emailService { get; }
    public IOptions<EmailOptionsDTO> _emailOptions { get; }

    public AuthController(IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager, IEmail emailService, IOptions<EmailOptionsDTO> emailOptions)
    {
      _emailOptions = emailOptions;
      _emailService = emailService;
      _config = config;
      _userManager = userManager;
      _signInManager = signInManager;
    }

    // Post api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewmodel model)
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

    // Post api/Auth/resetpassword
    [HttpPost("resetpassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewmodel model)
    {
      var user = await _userManager.FindByEmailAsync(model.Email);
      if (user != null || user.EmailConfirmed)
      {
        // Send confirmation email.
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var changePasswordUrl = Request.Headers["changeEmailUrl"]; //http://localhost:4200/change-password

        var uriBuilder = new UriBuilder(changePasswordUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["token"] = token;
        query["userid"] = user.Id;
        uriBuilder.Query = query.ToString();
        var urlString = uriBuilder.ToString();

        var emailBody = $"<p>Click on the link below to change your password.</p><p>{urlString}</p>";
        await _emailService.SendAsync(model.Email, emailBody, _emailOptions.Value);

        return Ok();
      }
      return Unauthorized();
    }

    // Post api/Auth/changepassword
    [HttpPost("changepassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewmodel model)
    {
      var user = await _userManager.FindByIdAsync(model.UserId);
      var resetPasswordResult = await _userManager.ResetPasswordAsync(user, Uri.UnescapeDataString(model.Token), model.Password);

      if (resetPasswordResult.Succeeded)
      {
        return Ok();
      }

      return Unauthorized();
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