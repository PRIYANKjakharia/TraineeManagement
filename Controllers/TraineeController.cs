using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Models;
using TraineeManagement.API.Services;

namespace TraineeManagement.Api.Controllers
{
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
        public async Task<IActionResult> Post(CreateTraineeRequest T)
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
            if (trainee == null) return NotFound();
            return Ok(trainee);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.Delete(id);
            if (!success) return NotFound();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , UpdateTraineeRequest request)
        {
            var success = await _service.Update(id, request);
            if (!success) return NotFound();
            return Ok();
        }
    }
}