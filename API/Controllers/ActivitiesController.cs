using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Application.Activities;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
            //to get our response back from the mediator
        }
        
        [HttpGet("{id}")]//get route parameter using curly brackets activities/id
        //{Id = id} object initializer syntax (setting Id to id)
        public async Task<IActionResult> GetActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query{Id = id}));
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity){
            return HandleResult(await Mediator.Send(new Create.Command {Activity = activity}));
        }

        //To only permit host to edit the activity
        [Authorize (Policy = "IsActivityHost") ]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity){
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command{Activity = activity}));
        }

        //To only permit host to edit the activity
        [Authorize (Policy = "IsActivityHost") ]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id){
            return HandleResult(await Mediator.Send(new Delete.Command{Id=id}));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend (Guid id){
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command{Id = id}));
        }
    }
}