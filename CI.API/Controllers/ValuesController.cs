using System.Collections.Generic;
using CI.DAL;
using CI.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CI.API.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ValuesController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: api/values http://localhost:5000/api/values
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> Get() {
            var values = await _context.Values.ToListAsync();
            return Ok(values);
        }

        // GET: api/values/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) {
        var value = await _context.Values.FindAsync(id);
        return Ok(value);
    }

        //POST: api/values
        [HttpPost]
        public void Post([FromBody]string value) {

        }

        //PUT: api/values
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value) {
            
        }

        //DELETE: api/values/5
        [HttpDelete]
        public void Delete(int id) {
            
        }
    }
}