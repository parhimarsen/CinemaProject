using CinemaProject.BLL.Models;
using CinemaProject.BLL.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly FoodService _foodService;

        public FoodsController(FoodService foodService)
        {
            _foodService = foodService;
        }

        // GET: api/Foods
        [HttpGet]
        [EnableQuery]
        public IQueryable<Food> GetFoods()
        {
            return _foodService.GetAll();
        }

        // POST: api/Foods
        [HttpPost]
        public async Task<IActionResult> PostFood(Food model)
        {
            Food response = await _foodService.InsertAsync(model);

            return Ok(response);
        }

        // PUT: api/Foods
        [HttpPut]
        public async Task<IActionResult> PutTicket(Food model)
        {
            await _foodService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(Guid id)
        {
            await _foodService.RemoveAsync(id);

            return NoContent();
        }
    }
}
