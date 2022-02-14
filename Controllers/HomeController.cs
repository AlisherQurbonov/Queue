using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using queue.Data;
using queue.Entities;
using queue.Models;
using queue.ViewModels;

namespace queue.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    private readonly UserManager<User> _usmanager;
    private readonly SignInManager<User> _signin;

    public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _logger = logger;
        _context = context;
        _usmanager = userManager;
        _signin = signInManager;
    }


    [HttpGet]
    public IActionResult Index()
    {
        var queues = _context.Registers.ToList().OrderBy(i => i.CreatedAt).ToList();


        return View(queues.ToModel());
    }

    [HttpPost]

    public IActionResult TakeQueue([FromForm] RegisterViewModel model)
    {
        var order = _context.Registers.Where(o => o.Active == true).OrderBy(p => p.CreatedAt).LastOrDefault();

        if (!ModelState.IsValid)
        {
            return BadRequest($"{ModelState.ErrorCount} errors detected!");
        }
        var user = model.ToEntity();

        if (order == null)
        {
           user.ModefiedAt = DateTimeOffset.UtcNow.ToLocalTime();
            user.Queue = user.ModefiedAt.AddMinutes(15);
        }
        if (order == null || order.ModefiedAt < DateTimeOffset.UtcNow.ToLocalTime() || order.Active == false)
        {
            user.ModefiedAt = DateTimeOffset.UtcNow.ToLocalTime();
            user.Queue = user.ModefiedAt.AddMinutes(15);
            user.Active = model.Active = true;

        }
        else
        {
            user.ModefiedAt = order.ModefiedAt.AddMinutes(15);
            user.Queue = user.ModefiedAt;
            user.Active = model.Active = true;
        }

        try
        {
            _context.Registers.Add(user);
            _context.SaveChanges();
        }

        catch (Exception e)
        {
            _logger.LogCritical($"Error in adding queue: {e.Message}");
        }


        return RedirectToAction("ShowQueue", user);

    }

    [HttpGet("{Id}/showqueue")]
    public async Task<IActionResult> ShowQueue([FromRoute]Guid Id)
    {
        if (! await _context.Registers.AnyAsync(c => c.Id == Id))
        {
            return NotFound();
        }

        var user = _context.Registers.FirstOrDefault(u=>u.Id==Id);
       
        if (user == default)
        {
            return NotFound();
        }

        return View(user.ToModel());
    }

    [HttpGet]

    public IActionResult TakeQueue()
    {
        return View();
    }


    [HttpGet]
    public IActionResult Admin(string returnUrl)
        => View(new LoginViewModel() { ReturnUrl = returnUrl ?? string.Empty });

    [HttpPost]
    public async Task<IActionResult> Admin(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _usmanager.Users.FirstOrDefault(u => u.Email == model.Email);
        if (user == default)
        {
            ModelState.AddModelError("Password", "Email yoki parol noto'g'ri kiritilgan.");
            return View(model);
        }

        var result = await _signin.PasswordSignInAsync(user, model.Password, false, false);
        if (result.Succeeded)
        {
            return LocalRedirect(model.ReturnUrl ?? "/");
        }

        return BadRequest(result.IsNotAllowed);
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


public static class EntityModelMappers
{

    public static RegisterListViewModel ToModel(this List<Register> entity)
    {
        return new RegisterListViewModel()
        {
            Registers = entity.Select(i =>
            {
                return new RegisterViewModel()
                {
                    Id = i.Id,
                    Fullname = i.Fullname,
                    Phone = i.Phone,
                    CreatedAt = i.CreatedAt,
                    ModefiedAt = i.ModefiedAt,
                    Queue = i.Queue,
                    Active = i.Active
                };
            }).ToList()
        };
    }

    public static RegisterViewModel ToModel(this Register entity)
    {
        return new RegisterViewModel()
        {

            Id = entity.Id,
            Fullname = entity.Fullname,
            Phone = entity.Phone,
            CreatedAt = entity.CreatedAt,
            ModefiedAt = entity.ModefiedAt,
            Queue = entity.Queue,
            Active = entity.Active

        };
    }


    public static Entities.Register ToEntity(this ViewModels.RegisterViewModel model)
      => new Entities.Register()
      {
          Id = Guid.NewGuid(),
          Fullname = model.Fullname,
          CreatedAt = DateTimeOffset.UtcNow.ToLocalTime(),
          Phone = model.Phone,
          Active = true,
        
      };
}
