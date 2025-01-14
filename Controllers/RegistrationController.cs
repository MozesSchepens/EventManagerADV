using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EventManagerADV.Models;
using EventManagerADV.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System;

public class RegistrationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;

    public RegistrationController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    // GET: /Registration/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Registration/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                Voornaam = model.Voornaam,
                Achternaam = model.Achternaam
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Generate email confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(
                    nameof(ConfirmEmail),
                    "Registration",
                    new { userId = user.Id, token = token },
                    Request.Scheme);

                // Send confirmation email
                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Bevestig je e-mailadres",
                    $"Gelieve je account te bevestigen door <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>hier te klikken</a>.");

                TempData["SuccessMessage"] = "Registratie geslaagd! Controleer je e-mail om je account te bevestigen.";
                return RedirectToAction("RegisterConfirmation");
            }

            // Voeg foutmeldingen toe aan ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    // GET: /Registration/RegisterConfirmation
    [HttpGet]
    public IActionResult RegisterConfirmation()
    {
        return View();
    }

    // GET: /Registration/ConfirmEmail
    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            return BadRequest("Ongeldig bevestigingsverzoek.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("Gebruiker niet gevonden.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Je e-mailadres is succesvol bevestigd. Je kunt nu inloggen.";
            return RedirectToAction("Login", "Account");
        }

        return BadRequest("Het bevestigen van je e-mailadres is mislukt.");
    }
}
