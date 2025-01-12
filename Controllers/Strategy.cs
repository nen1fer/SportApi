using SportApi.Models;

namespace SportApi.Controllers
{
    public interface IFilterStrategy<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items);
    }

    public class DurationFilter : IFilterStrategy<Workout>
    {
        private readonly int _minDuration;

        public DurationFilter(int minDuration)
        {
            _minDuration = minDuration;
        }

        public IEnumerable<Workout> Filter(IEnumerable<Workout> items)
        {
            return items.Where(w => w.Duration >= _minDuration);
        }
    }


    public class MinWorkoutCountFilter : IFilterStrategy<WorkoutProgram>
    {
        private readonly int _minWorkouts;

        public MinWorkoutCountFilter(int minWorkouts)
        {
            _minWorkouts = minWorkouts;
        }

        public IEnumerable<WorkoutProgram> Filter(IEnumerable<WorkoutProgram> items)
        {
            return items.Where(p => p.Workouts.Count >= _minWorkouts);
        }
    }

    public class FilterContext<T>
    {
        private IFilterStrategy<T> _strategy;

        public void SetStrategy(IFilterStrategy<T> strategy)
        {
            _strategy = strategy;
        }

        public IEnumerable<T> ApplyFilter(IEnumerable<T> items)
        {
            if (_strategy == null)
            {
                throw new InvalidOperationException("Filter strategy is not set.");
            }
            return _strategy.Filter(items);
        }
    }

}
