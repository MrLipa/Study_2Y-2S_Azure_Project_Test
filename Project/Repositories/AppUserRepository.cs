using Project.Data;
using Project.Interfaces;
using Project.Models;


namespace Project.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly DataContext _context;

        public AppUserRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<AppUser> GetAllAppUsers()
        {
            return _context.AppUsers.OrderBy(p => p.UserId).ToList();
        }

        public AppUser GetAppUserById(int id)
        {
            var user = _context.AppUsers.Find(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"Użytkownik o ID {id} nie został znaleziony.");
            }

            return user;
        }

        public AppUser GetAppUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email nie może być pusty.");
            }

            var user = _context.AppUsers.FirstOrDefault(u => u.Email == email);

            return user;
        }

        public void AddAppUser(AppUser appUser)
        {
            _context.AppUsers.Add(appUser);
            _context.SaveChanges();
        }

        public void UpdateAppUser(AppUser appUser)
        {
            _context.AppUsers.Update(appUser);
            _context.SaveChanges();
        }

        public void DeleteAppUser(int id)
        {
            var appUser = _context.AppUsers.Find(id);
            if (appUser != null)
            {
                _context.AppUsers.Remove(appUser);
                _context.SaveChanges();
            }
        }
        public ICollection<Meal> GetMealsByUserId(int userId)
        {
            var meals = _context.UserMeals
                .Where(um => um.UserId == userId)
                .Select(um => um.Meal)
                .ToList();

            return meals;
        }

        public IEnumerable<Meal> GetMealsByUserAndDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            return _context.UserMeals
                           .Where(um => um.UserId == userId && um.ConsumedAt >= startDate && um.ConsumedAt <= endDate)
                           .Select(um => um.Meal)
                           .ToList();
        }
    }
}
