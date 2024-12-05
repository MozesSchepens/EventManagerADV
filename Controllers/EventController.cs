using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagerADV.Data;
using EventManagerADV.Models;

namespace EventManagerADV.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index: Lijst van evenementen
        public IActionResult Index()
        {
            var events = _context.Events
                .Include(e => e.Category) // Inclusief categorie
                .Include(e => e.EventVolunteers) // Inclusief koppeltabel
                    .ThenInclude(ev => ev.Volunteer) // Inclusief vrijwilliger
                .Where(e => !e.IsDeleted) // Alleen niet-gedelete evenementen
                .ToList();

            return View(events);
        }

        // Details: Details van een evenement
        public IActionResult Details(int id)
        {
            var @event = _context.Events
                .Include(e => e.Category) // Inclusief categorie
                .Include(e => e.EventVolunteers) // Inclusief koppeltabel
                    .ThenInclude(ev => ev.Volunteer) // Inclusief vrijwilliger
                .FirstOrDefault(e => e.Id == id && !e.IsDeleted);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // Create: Tonen van create-formulier
        public IActionResult Create()
        {
            ViewData["Categories"] = _context.Categories.ToList();
            ViewData["Volunteers"] = _context.Volunteers.ToList();
            return View();
        }

        // Create: Opslaan van een nieuw evenement
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event @event, int[] selectedVolunteers)
        {
            if (ModelState.IsValid)
            {
                // Voeg geselecteerde vrijwilligers toe
                foreach (var volunteerId in selectedVolunteers)
                {
                    var eventVolunteer = new EventVolunteer
                    {
                        Event = @event,
                        VolunteerId = volunteerId
                    };
                    _context.EventVolunteers.Add(eventVolunteer);
                }

                _context.Events.Add(@event);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categories"] = _context.Categories.ToList();
            ViewData["Volunteers"] = _context.Volunteers.ToList();
            return View(@event);
        }

        // Edit: Tonen van edit-formulier
        public IActionResult Edit(int id)
        {
            var @event = _context.Events
                .Include(e => e.EventVolunteers) // Inclusief koppeltabel
                .FirstOrDefault(e => e.Id == id && !e.IsDeleted);

            if (@event == null)
            {
                return NotFound();
            }

            ViewData["Categories"] = _context.Categories.ToList();
            ViewData["Volunteers"] = _context.Volunteers.ToList();
            return View(@event);
        }

        // Edit: Opslaan van wijzigingen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Event @event, int[] selectedVolunteers)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Update vrijwilligers
                var existingEventVolunteers = _context.EventVolunteers.Where(ev => ev.EventId == id).ToList();
                _context.EventVolunteers.RemoveRange(existingEventVolunteers);

                foreach (var volunteerId in selectedVolunteers)
                {
                    var eventVolunteer = new EventVolunteer
                    {
                        EventId = id,
                        VolunteerId = volunteerId
                    };
                    _context.EventVolunteers.Add(eventVolunteer);
                }

                _context.Events.Update(@event);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Categories"] = _context.Categories.ToList();
            ViewData["Volunteers"] = _context.Volunteers.ToList();
            return View(@event);
        }

        // Delete: Soft-delete
        public IActionResult Delete(int id)
        {
            var @event = _context.Events.FirstOrDefault(e => e.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            @event.IsDeleted = true;
            _context.Events.Update(@event);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
