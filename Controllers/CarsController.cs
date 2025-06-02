using Microsoft.AspNetCore.Mvc;
using ApiService.Models;

namespace MySimApiServicepleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var cars = new[] {
new Car { CarNumber = "A777AA", Model = "Opel" },
new Car { CarNumber = "T222TT", Model = "Lada" },
new Car { CarNumber = "o000oo", Model = "Mazda" }
};
            return Ok(cars);
        }
    }
}