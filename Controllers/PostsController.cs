using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PullDataApi.Models;
using PullDataApi.Persistance;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PullDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DataContext _context;
        public PostsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/<PostsController>
        [HttpGet]
        public IActionResult Get()
        {
            var posts = _context.Posts.ToList();
            return Ok(new Response<IEnumerable<Post>>(posts));
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var post = _context.Posts.ToList()
                .Where(p => p.Id == id).FirstOrDefault();
            if (post == null)
                return BadRequest(new Response<Post>
                {
                    Succeeded = false,
                    Message = $"No posts with id: {id}"
                });
            return Ok(new Response<Post>(post));
        }

        /*// POST api/<PostsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PostsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
