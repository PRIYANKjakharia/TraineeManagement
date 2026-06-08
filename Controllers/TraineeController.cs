using Microsoft.AspNetCore.Mvc;
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
            return Ok(_service.Create(T));
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
                return Ok(_service.GetAll());
            }
        }

        [HttpGet("{id}")]
        public Task<TraineeResponse> GetById(int id)
        {
            // var trainee = trainees.FirstOrDefault(x => x.Id == id);
            // if(trainee == null)
            // {
            //     return NotFound();
            // }
            // return Ok(trainee);

            // foreach(Trainee t in trainees)
            // {
            //     if(t.Id == id)return Ok(t);
            // }
            // return NotFound();
            return _service.GetById(id);
        }
        [HttpDelete("{id}")]
        public Task<bool> Delete(int id)
        {
            return _service.Delete(id);
        }
        [HttpPut("{id}")]
        public Task<bool> Update(int id , UpdateTraineeRequest request)
        {
            return _service.Update(id , request);
        }


        // [HttpGet]
        // public async Task<IActionResult> Search(String search)
        // {
        //     var t=await _service.Search(search);
        //     if(t==null) return NotFound();
        //     else return Ok(t);
        // }
    }
}