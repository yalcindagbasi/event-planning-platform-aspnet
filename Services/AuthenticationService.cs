// Services/AuthenticationService.cs
using SmartEventPlanningPlatform.Data;
using SmartEventPlanningPlatform.Models;
using System.Reflection;

namespace SmartEventPlanningPlatform.Services
{
    public class AuthenticationService
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Kullanıcı adı ve şifreyle giriş yapma
        public User Authenticate(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.PasswordHash == password);

            return user;
        }
        public User Register(string username, string password, string email, DateTime createdDate, string firstName,string lastName,string phoneNumber,string location,string aboutme,DateTime birthDate,string gender,string profilePictureUrl, string Latitude, string Longitude,List<string> SelectedInterests)
        {
            

            var user = new User
            {
                Username = username,
                PasswordHash = password,
                Email = email,
                CreatedAt = createdDate.ToUniversalTime(),
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Location = location,
                AboutMe= aboutme,
                Interests = string.Join(", ", SelectedInterests),
                BirthDate = birthDate.ToUniversalTime(),
                Gender = gender,
                ProfilePictureUrl = profilePictureUrl ?? "https://i.pinimg.com/236x/4a/cf/16/4acf16a2999a4c6dfdfe03f198b95b13.jpg",
                Latitude = Latitude,
                Longitude = Longitude
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
    }
}
