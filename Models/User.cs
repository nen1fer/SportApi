namespace SportApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? DateOfBirth { get; set; } // Дата рождения
        public float? Weight { get; set; } // Вес
        public float? Height { get; set; } // Рост
        public ICollection<int> FavoriteWorkoutIds { get; set; } = new List<int>(); // Избранные тренировки
    }
}

