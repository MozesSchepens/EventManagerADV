using Microsoft.AspNetCore.Mvc;
using EventManagerADV.Models;
using EventManagerADV.Data;
using Microsoft.EntityFrameworkCore;

namespace EventManagerADV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var events = _context.Events
                .Include(e => e.Category) 
                .Where(e => !e.IsDeleted) 
                .ToList();
            return View(events);
        }
    }
}
