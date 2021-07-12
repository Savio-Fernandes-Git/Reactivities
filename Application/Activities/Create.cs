using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        // commands dont return anything so no type operator
        public class Command : IRequest
        {
            public Activity Activity { get; set; } //we receive as a parameter from our API
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;

            }

            //unit is an object that mediatr provides but doesn't really have any value, tells our API the request is finished so it can move on
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {   
                //we could use async but it isnt required
                _context.Activities.Add(request.Activity);

                await _context.SaveChangesAsync();

                return Unit.Value; 
            }
        }
    }
}