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

            //Selecting who views the comments
            //SendAsync(string functionName)
            //we send comment.Value because comment is a result object that contains our commentDto, so we must get the value of that
            await Clients.Group(command.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value);
        }
    
        //Overriding method inside the hub
        //SignalR automatically disconnects the session after client disconnection
        //after client connects, we're gonna join them to a group of the activityId and we'll send them a list of comments
        public override async Task OnConnectedAsync()
        {
            //getting activityId from query string
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            
            //Object Groups available from SignalR hub. AddToGroupAsync(connectionName, activityName) allows you to add a connected user to a group
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
            
            //to send down list of comments to client
            var result = await _mediator.Send(new List.Query{ActivityId = Guid.Parse(activityId)});
            
            //SendAsync sends this request tto person making this call (Client.Caller)
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}