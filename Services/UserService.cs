using SmartEventPlanningPlatform.Data;
using SmartEventPlanningPlatform.Models;

namespace project.web.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<User> getAllUsers()
        {
            return _context.Users.ToList();
        }
        public User getUserDetailsbyId(int userId)
        {
            return _context.Users.Find(userId);
        }
        public User getUserDetailsbyUserName(string username)
        {
            return _context.Users.Find(username);
        }
        public User createUser(string username, string password, string email, DateTime createdTime, string firstName, string lastName,
    string phoneNumber, string location, string aboutme, DateTime birthDate, string gender, string profilePictureUrl, List<string> interests)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                return null;
            }

            var user = new User
            {
                Username = username,
                PasswordHash = password,
                Email = email,
                CreatedAt = createdTime,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Location = location,
                AboutMe = aboutme,
                Interests = string.Join(", ", interests),
                BirthDate = birthDate,
                Gender = gender,
                ProfilePictureUrl = profilePictureUrl ?? "https://i.pinimg.com/236x/4a/cf/16/4acf16a2999a4c6dfdfe03f198b95b13.jpg"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User updateUser(User user) {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }
        public void deleteUser(User user)
        {
            // Kullanıcının tüm katılımcılıklarını sil
            var participants = _context.Participants.Where(p => p.UserId == user.UserId).ToList();
            _context.Participants.RemoveRange(participants);

            // Kullanıcı tarafından gönderilen tüm mesajları sil
            var messages = _context.Messages.Where(m => m.SenderId == user.UserId).ToList();
            _context.Messages.RemoveRange(messages);

            // Kullanıcıyı sil
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

    }
}
