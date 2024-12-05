using System.Collections.Generic;

namespace EventManagerADV.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Email { get; set; }

        public ICollection<EventVolunteer> EventVolunteers { get; set; } = new List<EventVolunteer>();
    }
}
