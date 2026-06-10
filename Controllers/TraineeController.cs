using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Services;

namespace TraineeManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/trainees")]
    public class TraineeController : ControllerBase
    {
        private readonly ITraineeService _service;
        public TraineeController(ITraineeService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTraineeRequest T)
        {
            var result = await _service.Create(T);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll( String? search)
        {
            if (!String.IsNullOrWhiteSpace(search))
            {
                var t=await _service.Search(search);
                if(t==null) return NotFound();
                else return Ok(t);
            } else {
                var result = await _service.GetAll();
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var trainee = await _service.GetById(id);
            if (trainee == null) return NotFound( new{ message = "id not found"} );
            return Ok(trainee);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.Delete(id);
            if (!success) return NotFound( new{ message = "Sucessfully deleted"} );
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , UpdateTraineeRequest request)
        {
            var success = await _service.Update(id, request);
            if (!success) return NotFound(new{ message = "Id Not Found"});
            return Ok( new{ message = "Sucessfully updated"} );
        }
    }
}