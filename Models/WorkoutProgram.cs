using System.Collections.Generic;
namespace SportApi.Models
{
        public class WorkoutProgram
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<int> WorkoutIds { get; set; } = new List<int>();
            public List<Workout>? Workouts { get; set; }
        }
}
