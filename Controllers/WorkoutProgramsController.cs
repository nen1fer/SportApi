using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportApi.Models;

namespace SportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutProgramsController : ControllerBase
    {
        private readonly WorkoutContext _context;
        private readonly WorkoutProgramFactory _factory;
        private readonly WorkoutProgramRepository _repository;
        public WorkoutProgramsController(WorkoutProgramRepository repository, WorkoutProgramFactory factory, WorkoutContext context)
        {
            _repository = repository;
            _factory = factory;
            _context = context;
        }

        // GET: api/workoutprograms
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutProgram>>> GetWorkoutPrograms()
        {
            var programs = _repository.GetAll();
            return Ok(programs);
        }

        // GET: api/workoutprograms/{id}
        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutProgram>> GetWorkoutProgram(int id)
        {
            var program = _repository.GetById(id);

            if (program == null)
            {
                return NotFound();
            }

            return Ok(program);
        }

        // GET: api/workoutprograms/filter
        //[Authorize]
        [HttpGet("workoutprograms/filter")]
        public ActionResult<IEnumerable<WorkoutProgram>> FilterWorkoutPrograms(int? minWorkouts, string? keyword)
        {
            var context = new FilterContext<WorkoutProgram>();

            if (minWorkouts.HasValue)
            {
                context.SetStrategy(new MinWorkoutCountFilter(minWorkouts.Value));
            }
            else
            {
                return BadRequest("No filter criteria provided.");
            }

            var filteredPrograms = context.ApplyFilter(_context.WorkoutPrograms.Include(p => p.Workouts));
            return Ok(filteredPrograms);
        }

        // POST: api/WorkoutPrograms
        //[Authorize]
        [HttpPost]
        public ActionResult<WorkoutProgram> PostWorkoutProgram([FromBody] WorkoutProgram workoutProgram)
        {
            try
            {
                var newProgram = _factory.CreateProgram(workoutProgram.Name, workoutProgram.Description, workoutProgram.WorkoutIds);
                _repository.Add(newProgram);
                _repository.Save();

                return CreatedAtAction(nameof(PostWorkoutProgram), new { id = newProgram.Id }, newProgram);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // PUT: api/WorkoutPrograms/{id}
        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkoutProgram(int id, WorkoutProgram workoutProgram)
        {
            if (id != workoutProgram.Id)
            {
                return BadRequest();
            }

            _repository.Update(workoutProgram);
            _repository.Save();

            return NoContent();
        }

        // DELETE: api/WorkoutPrograms/{id}
        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutProgram(int id)
        {
            var workoutProgram = _repository.GetById(id);
            if (workoutProgram == null)
            {
                return NotFound();
            }

            _repository.Delete(workoutProgram);
            _repository.Save();

            return NoContent();
        }
    }
}
