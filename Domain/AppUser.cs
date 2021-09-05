using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        
        //forming the relationship
        public ICollection<ActivityAttendee> Activities { get; set; }
    }
}