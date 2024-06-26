using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ChattApp.Models;
using Microsoft.EntityFrameworkCore;


namespace ChattApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ChattAppDb _context;
    private const string UserKey = "USER_KEY";

    public HomeController(ILogger<HomeController> logger, ChattAppDb context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userName = HttpContext.Session.GetString(UserKey);
        if (string.IsNullOrEmpty(userName))
        {
            return RedirectToAction("SignIn");
        }

        var messages = await _context.Messages
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

    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignIn(SignInVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        SignInUser(vm.UserName);
        return RedirectToAction("Index");
    }

    private void SignInUser(string vmUserName)
    {
        HttpContext.Session.SetString(key:UserKey, value: vmUserName);
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