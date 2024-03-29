using System;
using System.Threading.Tasks;
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(Create.Command command)
        {
            var comment = await _mediator.Send(command);

            await Clients.Group(command.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value);
        }
    
        //Overriding method inside the hub
        //SignalR automatically disconnects the session after clinet disconnection
        //after client connects, we're gonna join them to a group of the activityId and we'll send them a list of comments
        public override async Task OnConnectedAsync()
        {
            //getting activityId from query string
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
            
            var result = await _mediator.Send(new List.Query{ActivityId = Guid.Parse(activityId)});
            
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}