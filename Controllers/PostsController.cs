using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PullDataApi.Helpers;
using PullDataApi.Models;
using PullDataApi.Persistance;
using PullDataApi.Services;
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
        private readonly IUriService _uriService;
        public PostsController(DataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }        

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Posts
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _context.Posts.CountAsync();
            var pagedResponse = PaginationHelper.CreatePagedReponse<Post>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedResponse);
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await  _context.Posts
                .Where(p => p.Id == id).FirstOrDefaultAsync();
            if (response == null)
                return BadRequest(new Response<Post>
                {
                    Succeeded = false,
                    Message = $"No posts with id: {id}"
                });
            return Ok(new Response<Post>(response));
        }        
    }
}
