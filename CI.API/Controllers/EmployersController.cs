using System.Text;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Collections.Generic;
using CI.DAL;
using CI.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CI.API.ViewModels;
using Microsoft.Extensions.Options;
using CI.SER.DTOs;
using CI.SER.Interfaces;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace CI.API.Controllers
{

  [ApiController]
  [Route("api/{controller}")]
  public class EmployersController : ControllerBase
  {
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IOptions<EmailOptionsDTO> _emailOptions;
    private readonly IEmail _emailService;
    private readonly IConfiguration _config;

    public EmployersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<EmailOptionsDTO> emailOptions, IEmail emailService, IConfiguration config)
    {
      _config = config;
      _emailService = emailService;
      _emailOptions = emailOptions;
      _roleManager = roleManager;
      _userManager = userManager;
    }

    // POST: api/employers/create http://localhost:5000/api/employers/create
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateEmployerViewmodel model)
    {
      // TODO: move this to Seed method.
      // TODO: set up enum for role names.
      if (!await _roleManager.RoleExistsAsync("Employer"))
      {
        await _roleManager.CreateAsync(new IdentityRole("Employer"));
      }

      var employer = new User
      {
        UserName = model.Username,
        Email = model.Email
      };
      var result = await _userManager.CreateAsync(employer, model.Password);

      if (!result.Succeeded)
      {
        return BadRequest(result);
      }

      await SendConfirmationEmail(employer);
      return Ok();

    }

    private async Task SendConfirmationEmail(User employer)
    {
      // Send confirmation email.
      var token = await _userManager.GenerateEmailConfirmationTokenAsync(employer);
      var confirmEmailUrl = Request.Headers["confirmEmailUrl"];//http://localhost:4200/email-confirm

      var uriBuilder = new UriBuilder(confirmEmailUrl);
      var query = HttpUtility.ParseQueryString(uriBuilder.Query);
      query["token"] = token;
      query["userid"] = employer.Id;
      uriBuilder.Query = query.ToString();
      var urlString = uriBuilder.ToString();

      var emailBody = $"<p>Please confirm your email by clicking on the link below.</p><p>{urlString}</p>";
      await _emailService.SendAsync(employer.Email, emailBody, "Please confirm your account", _emailOptions.Value);
      //////////////////////////////

      var user = await _userManager.FindByNameAsync(employer.UserName);
      await _userManager.AddToRoleAsync(user, "Employer");
    }

    //POST: api/values
    [HttpPost]
    public void Post([FromBody] string value)
    {

    }

    //PUT: api/values
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CreateEmployerViewmodel model)
    {
      try
      {
        var user = await GetUser();
        if (user != null)
        {
          user.Email = model.Email;
          user.EmailConfirmed = false;
          await _userManager.UpdateAsync(user);

          await SendConfirmationEmail(user);
          return Ok(user);
        }
        else
        {
          return BadRequest(model);
        }
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }

    //DELETE: api/values/5
    [HttpDelete]
    public void Delete(int id)
    {

    }

    private async Task<User> GetUser()
    {
      string userId = User.Claims.First(c => c.Type == "UserID").Value;

      return await _userManager.FindByIdAsync(userId);
    }

  }
}