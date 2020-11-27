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

    public EmployersController(UserManager<User> userManager)
    {
      this._userManager = userManager;

    }

    // POST: api/employers/create http://localhost:5000/api/employers/create
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateEmployerViewModel model)
    {
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