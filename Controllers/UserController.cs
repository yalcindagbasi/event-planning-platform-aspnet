using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.web.Services;
using SmartEventPlanningPlatform.Data;
using SmartEventPlanningPlatform.Models;
using SmartEventPlanningPlatform.Services;
using System.Security.Claims;

namespace SmartEventPlanningPlatform.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly GamificationService _gamificationService;
        public UserController(UserService userService, GamificationService gamificationService)
        {
            _userService = userService;
            _gamificationService = gamificationService;
        }

        [Authorize]
        public IActionResult Profile(int userId)
        {
            if(userId == 0)
            {
                userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            ViewBag.IsOwner = false;
            if(User.FindFirstValue(ClaimTypes.NameIdentifier) == userId.ToString())
            {
                ViewBag.IsOwner = true;
            }
            var userDetails = _userService.getUserDetailsbyId(userId);
            var userPoints = _gamificationService.GetUserPoints(userId);
            if (userPoints != null)
            {
                ViewBag.TotalPoints = userPoints.TotalPoints;
                ViewBag.EventParticipationPoints = userPoints.EventParticipationPoints;
                ViewBag.EventCreationPoints = userPoints.EventCreationPoints;
                ViewBag.BonusPoints = userPoints.BonusPoints;
            }
            

            ViewData["ActivePage"] = "Profile";
            return View(userDetails);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int userId)
        {
            var currentUserId= int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 0)
            {
                userId = currentUserId;
            }
            if (userId != currentUserId)
            {
                return Unauthorized();
            }
            var user = _userService.getUserDetailsbyId(userId);
            ViewBag.userId = userId;
            TempData["SuccessMessage"] = null;
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // User modelini View'a gönder
        }

        // Profil Güncelleme POST Metodu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Username,string Email,string ProfilePictureUrl,int userId)
        {
            
            if(userId == 0)
            {
                userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var user =  _userService.getUserDetailsbyId(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Kullanıcının bilgilerini güncelle
            user.Username = Username;
            user.Email = Email;
            user.ProfilePictureUrl = ProfilePictureUrl;

            _userService.updateUser(user); // Veritabanına kaydet

            TempData["SuccessMessage"] = "Profil başarıyla güncellendi!";
            return RedirectToAction("Profile");
        }

        
        public IActionResult Delete(int userId)
        {
            var user = _userService.getUserDetailsbyId(userId);
            if (user == null)
            {
                return NotFound();
            }

            _userService.deleteUser(user);
            return RedirectToAction("Logout", "Account");
        }
    }
}
