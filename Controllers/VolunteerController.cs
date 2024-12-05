using EventManagerADV.Data;
using Microsoft.AspNetCore.Mvc;

public class VolunteerController : Controller
{
    private readonly ApplicationDbContext _context;

    public VolunteerController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var volunteers = _context.Volunteers.ToList();
        return View(volunteers);
    }
}
