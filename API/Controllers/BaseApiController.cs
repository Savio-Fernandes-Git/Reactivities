using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //api/activities
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        //??= a null correslessing operator (if null assign right of the operator)
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}