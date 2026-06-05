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
        public IActionResult Post(CreateTraineeRequest T)
        {
            return Ok(_service.Create(T));
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public TraineeResponse GetById(int id)
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
        public bool Delete(int id)
        {
            return _service.Delete(id);
        }
        [HttpPut("{id}")]
        public bool Update(int id , UpdateTraineeRequest request)
        {
            return _service.Update(id , request);
        }
    }
}