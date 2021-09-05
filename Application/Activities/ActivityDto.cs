using System;
using System.Collections.Generic;
using Application.Profiles;

namespace Application.Activities
{
    public class ActivityDto
    {
        public Guid Id { get; set; } //Guid can be used to generate from either server or client side
        //Entity framework will recognize Id as the primary key for the RDB (relational database) 
        //need to define one of the key as Id for conventions
        public string Title { get; set; }
        public DateTime Date{ get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public string HostUsername { get; set; }
        public bool IsCancelled { get; set; }
        public ICollection<Profile> Attendees { get; set; }
    }
}