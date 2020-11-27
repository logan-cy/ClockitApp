using System.Collections.Generic;
using CI.DAL;
using CI.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CI.API.ViewModels;

namespace CI.API.Controllers
{

  [ApiController]
  [Route("api/{controller}")]
  public class EmployersController : ControllerBase
  {
    private readonly UserManager<User> _userManager;
    public RoleManager<IdentityRole> _roleManager { get; }

    public EmployersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
      _roleManager = roleManager;
      _userManager = userManager;

    }

    // POST: api/employers/create http://localhost:5000/api/employers/create
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateEmployerViewModel model)
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

      var user = await _userManager.FindByNameAsync(employer.UserName);
      await _userManager.AddToRoleAsync(user, "Employer");

      return Ok(result);
    }

    //POST: api/values
    [HttpPost]
    public void Post([FromBody] string value)
    {

    }

    //PUT: api/values
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {

    }

    //DELETE: api/values/5
    [HttpDelete]
    public void Delete(int id)
    {

    }
  }
}