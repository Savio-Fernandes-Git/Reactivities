using System;
using System.Text.Json.Serialization;

namespace Application.Profiles
{
    public class UserActivityDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }

        //want to add a prop to our class to help us out but dont want to return to the client we can use [JsonIgnore]
        [JsonIgnore]
        public string HostUsername { get; set; }
    }
} 