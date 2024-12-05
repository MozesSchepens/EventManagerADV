using System;
using System.Collections.Generic;

namespace EventManagerADV.Models
{
    public class Event
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public DateTime Date { get; set; } 
        public bool IsDeleted { get; set; } 

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<EventVolunteer> EventVolunteers { get; set; } = new List<EventVolunteer>();
    }
}
