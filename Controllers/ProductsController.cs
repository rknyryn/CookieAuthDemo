using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookieAuthDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet("GetAll")]
        public IEnumerable<string> GetAll()
        {
            return new string[] { "product1", "product2", "product3" };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllAuthorized")]
        public IEnumerable<string> GetAllAuthorized()
        {
            return new string[] { "product1", "product2", "product3" };
        }
    }
}
