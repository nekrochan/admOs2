using Microsoft.AspNetCore.Mvc;
using ApiService.Models;

namespace ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var owners = new[] {
new Owner { Id = 0, OwnerName = "John Doe" },
new Owner { Id = 1, OwnerName = "Second owner" },
new Owner { Id = 2, OwnerName = "Third owner" }
};
            return Ok(owners);
        }
    }
}