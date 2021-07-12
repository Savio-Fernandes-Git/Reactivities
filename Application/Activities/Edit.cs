using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest
        {
            //get actiivty from db
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                //null correlessing operator (this works manually unlike automapper)
                //activity.Title = request.Activity.Title ?? activity.Title;

                //map from activities coming in request to activity from db
                _mapper.Map(request.Activity, activity);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}