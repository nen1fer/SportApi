using Microsoft.EntityFrameworkCore;
using SportApi.Models;

namespace SportApi.Controllers
{
    public interface IRepository<T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
    public class WorkoutRepository : IRepository<Workout>
    {
        private readonly WorkoutContext _context;

        public WorkoutRepository(WorkoutContext context)
        {
            _context = context;
        }

        public Workout GetById(int id) => _context.Workouts.Find(id);

        public IEnumerable<Workout> GetAll() => _context.Workouts.ToList();

        public void Add(Workout workout) => _context.Workouts.Add(workout);

        public void Update(Workout workout) => _context.Workouts.Update(workout);

        public void Delete(Workout workout) => _context.Workouts.Remove(workout);

        public void Save() => _context.SaveChanges();
    }

    public class WorkoutProgramRepository : IRepository<WorkoutProgram>
    {
        private readonly WorkoutContext _context;

        public WorkoutProgramRepository(WorkoutContext context)
        {
            _context = context;
        }

        public WorkoutProgram GetById(int id)
        {
            return _context.WorkoutPrograms
                .Include(p => p.Workouts)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<WorkoutProgram> GetAll()
        {
            return _context.WorkoutPrograms
                .Include(p => p.Workouts) 
                .ToList();
        }

        public void Add(WorkoutProgram program)
        {
            _context.WorkoutPrograms.Add(program);
        }

        public void Update(WorkoutProgram program)
        {
            _context.WorkoutPrograms.Update(program);
        }

        public void Delete(WorkoutProgram program)
        {
            _context.WorkoutPrograms.Remove(program);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

    public class UserRepository : IRepository<User>
    {
        private readonly WorkoutContext _context;

        public UserRepository(WorkoutContext context) 
        {
            _context = context; 
        }
        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
        public User GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }
        public void Add(User user)
        {
            _context.Users.Add(user);
        }
        public void Update(User user)
        {
            _context.Users.Update(user);
        }
        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
