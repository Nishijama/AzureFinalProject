using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChattApp.Models;
using Microsoft.AspNetCore.Authentication;
using BCrypt.Net;
namespace ChattApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChattAppDb _context;

        public HomeController(ILogger<HomeController> logger, ChattAppDb context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var messages = await _context.Messages
                .Include(m => m.User)
                .OrderByDescending(m => m.CreatedOn)
                .Take(10)
                .OrderBy(m => m.CreatedOn)
                .ToListAsync();

            var vm = new IndexVm
            {
                UserName = userName,
                Messages = messages
            };

            return View(vm);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == vm.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(vm.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(vm);
            }

            SignInUser(user);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new User
            {
                UserName = vm.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            SignInUser(user);

            return RedirectToAction("Index");
        }

        private void SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity)).Wait();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
