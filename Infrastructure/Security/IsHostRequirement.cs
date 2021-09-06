using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {

    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IsHostRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            //we want to use users id in this case primary key of our table is made up of a combiation of user ID and activity ID
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null) return Task.CompletedTask; //means unauthorized

            //get our activity ID converting from Guid object to Guid String 
            var activityId = Guid.Parse( _httpContextAccessor.HttpContext?.Request.RouteValues
                .SingleOrDefault( x => x.Key == "id" ).Value?.ToString()
                );

            //get attendee object
            var attendee = _dbContext.ActivityAttendees
                .AsNoTracking()
                .SingleOrDefaultAsync( x => x.AppUserId == userId && x.ActivityId == activityId ).Result;

            if ( attendee == null ) return Task.CompletedTask;

            if ( attendee.IsHost ) context.Succeed( requirement );

            return Task.CompletedTask;
        }
    }
}