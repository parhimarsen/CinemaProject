using CinemaProject.BLL.Models;
using CinemaProject.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaProject.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TypesOfSeatController : ControllerBase
    {
        private readonly TypeOfSeatService _typeOfSeatService;

        public TypesOfSeatController(TypeOfSeatService typeOfSeatService)
        {
            _typeOfSeatService = typeOfSeatService;
        }

        // GET: api/TypesOfSeat
        [HttpGet]
        public IActionResult GetTypesOfSeat()
        {
            List<TypeOfSeat> response =  _typeOfSeatService.GetAllAsync();

            return Ok(response);
        }

        // POST: api/TypesOfSeat
        [HttpPost]
        public async Task<IActionResult> PostTypeOfSeat(TypeOfSeat model)
        {
            TypeOfSeat response = await _typeOfSeatService.InsertAsync(model);

            return Ok(response);
        }


        // PUT: api/TypesOfSeat
        [HttpPut]
        public async Task<IActionResult> PutTypeOfSeat(TypeOfSeat model)
        {
            await _typeOfSeatService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/TypesOfSeat/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeOfSeat(Guid id)
        {
            await _typeOfSeatService.RemoveAsync(id);

            return NoContent();
        }
    }
}
