using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportApi.Models;
using SportApi.Services;

namespace SportApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly WorkoutContext _context;
        private readonly TokenService _tokenService;
        private readonly UserRepository _repository;

        public AuthController(WorkoutContext context, TokenService tokenService, UserRepository repository)
        {
            _context = context;
            _tokenService = tokenService;
            _repository = repository;
        }

        public class LoginRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _repository.GetByUsername(request.username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
            {
                return Unauthorized("Неверное имя пользователя или пароль.");
            }

            var token = _tokenService.GenerateToken(user.Id, user.Username);

            return Ok(new { Token = token, UserId = user.Id });
        }

        [HttpPost("register")]
        public IActionResult Register(string username, string password)
        {
            if (_repository.GetByUsername(username) != null)
            {
                return BadRequest("Пользователь с таким именем уже существует.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User { Username = username, PasswordHash = hashedPassword };
            _repository.Add(user);
            _repository.Save();

            return Ok("Регистрация прошла успешно.");
        }

        [Authorize]
        [HttpPost("{userId}/favorites/add")]
        public IActionResult AddFavoriteWorkout(int userId, int workoutId)
        {
            var user = _repository.GetById(userId);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            if (!user.FavoriteWorkoutIds.Contains(workoutId))
            {
                user.FavoriteWorkoutIds.Add(workoutId);
                _repository.Save();
            }

            return Ok("Тренировка добавлена в избранное.");
        }

        [Authorize]
        [HttpPost("{userId}/favorites/remove")]
        public IActionResult RemoveFavoriteWorkout(int userId, int workoutId)
        {
            var user = _repository.GetById(userId);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            if (user.FavoriteWorkoutIds.Contains(workoutId))
            {
                user.FavoriteWorkoutIds.Remove(workoutId);
                _repository.Save();
            }

            return Ok("Тренировка удалена из избранного.");
        }

        //[Authorize]
        [HttpPost("{userId}/update-profile")]
        public IActionResult UpdateProfile( int userId, [FromBody] ProfileUpdateRequest request)
        {
            Console.WriteLine($"UserId: {userId}, DateOfBirth: {request.dateOfBirth}, Weight: {request.weight}, Height: {request.height}");
            var user = _repository.GetById(userId);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            user.DateOfBirth = request.dateOfBirth;
            user.Weight = request.weight;
            user.Height = request.height;

            _repository.Save();
            return Ok("Профиль обновлен.");
        }

        public class ProfileUpdateRequest
        {
            public DateTime? dateOfBirth { get; set; }
            public float? weight { get; set; }
            public float? height { get; set; }
        }

        //[Authorize]
        [HttpGet("{userId}")]
        public IActionResult GetProfile(int userId)
        {
            var user = _repository.GetById(userId);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            var userProfile = new
            {
                user.Username,
                user.DateOfBirth,
                user.Weight,
                user.Height
            };

            return Ok(userProfile);
        }


    }

}
