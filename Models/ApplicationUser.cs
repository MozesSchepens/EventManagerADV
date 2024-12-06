using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagerADV.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
    }
}