using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<List<Activity>> {}//dont need dependencies when were just returning a list

        //implement interface using quick fix
        public class Handler : IRequestHandler<Query, List<Activity>>
        {
            private readonly DataContext _context;
            //use quickfix to initialize field from parameter and use persistence
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Activities.ToListAsync();
            }
        }
    }
}