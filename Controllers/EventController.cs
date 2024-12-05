using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventManagerADV.Data;
using EventManagerADV.Models;


namespace EventManagerADV.Controllers
{
    [Authorize] 
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Alleen toegankelijk voor Admin en User
        [Authorize(Roles = "Admin,User")]
        public IActionResult Index()
        {
            var events = _context.Events.ToList();
            return View(events);
        }

        // Alleen toegankelijk voor Admin
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Event eventModel)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(eventModel);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(eventModel);
        }
    }
}
