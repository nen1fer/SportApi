using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportApi.Models;

namespace SportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly WorkoutContext _context;
        private readonly WorkoutFactory _factory;
        private readonly WorkoutRepository _repository;
        public WorkoutsController(WorkoutRepository repository, WorkoutFactory factory, WorkoutContext context)
        {
            _repository = repository;
            _factory = factory;
            _context = context;
        }

        // GET: api/workouts
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkouts()
        {
            var workouts = _repository.GetAll();
            return Ok(workouts);
        }

        // GET: api/workouts/{id}
        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Workout>> GetWorkout(int id)
        {
            var workout = _repository.GetById(id);

            if (workout == null)
            {
                return NotFound();
            }

            return Ok(workout);
        }

        // GET: api/workouts/filter
        //[Authorize]
        [HttpGet("workouts/filter")]
        public ActionResult<IEnumerable<Workout>> FilterWorkouts(int? minDifficulty, int? minDuration)
        {
            var context = new FilterContext<Workout>();

            if (minDuration.HasValue)
            {
                context.SetStrategy(new DurationFilter(minDuration.Value));
            }
            else
            {
                return BadRequest("No filter criteria provided.");
            }

            var filteredWorkouts = context.ApplyFilter(_context.Workouts);
            return Ok(filteredWorkouts);
        }

        // POST: api/Workouts
        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<Workout>> PostWorkout(Workout workout)
        {
            try
            {
                var newWorkout = _factory.CreateWorkout(
                    workout.Name,
                    workout.Description,
                    workout.Duration);

                _repository.Add(newWorkout);
                _repository.Save();

                return CreatedAtAction(nameof(PostWorkout), new { id = newWorkout.Id }, newWorkout);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Workouts/{id}
        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkout(int id, Workout workout)
        {
            if (id != workout.Id)
            {
                return BadRequest();
            }

            _repository.Update(workout);
            _repository.Save();

            return NoContent();
        }

        // DELETE: api/Workouts/{id}
        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var workout = _repository.GetById(id);
            if (workout == null)
            {
                return NotFound();
            }

            _repository.Delete(workout);
            _repository.Save();

            return NoContent();
        }
    }
}
