using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        // commands dont return anything so no type operator
        public class Command : IRequest<Result<Unit>>
        {//we use unit when we're not returning anything. we're returning for the sake of validation here
            public Activity Activity { get; set; } //we receive as a parameter from our API
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }
        
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;

            }

            //unit is an object that mediatr provides but doesn't really have any value, tells our API the request is finished so it can move on
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {   
                //we could use async but it isnt required
                _context.Activities.Add(request.Activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create Activity");

                return Result<Unit>.Success(Unit.Value); 
            }
        }
    }
}