using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EventManagerADV.Models
{
    public class EditRolesViewModel
    {
        public string UserId { get; set; }
        public List<IdentityRole> AvailableRoles { get; set; } = new List<IdentityRole>();
        public List<string> UserRoles { get; set; } = new List<string>();
    }
}
