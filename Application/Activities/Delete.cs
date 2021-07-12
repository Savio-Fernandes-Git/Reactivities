using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;

            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //finds the activity
                var activity = await _context.Activities.FindAsync(request.Id);

                //removes activity
                _context.Remove(activity);

                //saves changes in db
                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}