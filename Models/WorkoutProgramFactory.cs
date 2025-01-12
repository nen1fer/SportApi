namespace SportApi.Models
{
    public class WorkoutProgramFactory
    {
        private readonly WorkoutContext _context;
        public WorkoutProgramFactory(WorkoutContext context)
        {
            _context = context;
        }
        public WorkoutProgram CreateProgram(string name, string description, List<int> workoutIds)
        {
            var workouts = _context.Workouts.Where(w => workoutIds.Contains(w.Id)).ToList();

            if (workouts.Count != workoutIds.Count)
            {
                throw new ArgumentException("Some workouts do not exist.");
            }

            return new WorkoutProgram
            {
                Name = name,
                Description = description,
                WorkoutIds = workoutIds,
                Workouts = workouts
            };
        }
    }
    public class WorkoutFactory
    {
        private readonly WorkoutContext _context;

        public WorkoutFactory(WorkoutContext context)
        {
            _context = context;
        }

        public Workout CreateWorkout(string name, string description, int duration)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Workout name cannot be empty.");
            }

            if (duration <= 0)
            {
                throw new ArgumentException("Duration must be greater than 0.");
            }

            return new Workout
            {
                Name = name,
                Description = description,
                Duration = duration
            };
        }
    }
}
