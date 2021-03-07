using AWS.Insurance.Operations.Application.Errors;
using AWS.Insurance.Operations.Application.Map;
using AWS.Insurance.Operations.Data.UoW;
using AWS.Insurance.Operations.Domain.Models;
using FluentValidation;
using MediatR;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Application.Cars
{
    public class Create
    {
        public class Command : IRequest
        {
            public Guid CustomerId { get; set; }
            public int BranchType { get; set; }
            public string Name { get; set; }
            public int Year { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.CustomerId).NotEmpty();
                RuleFor(x => x.BranchType).NotEmpty();
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Year).GreaterThan(1999);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var customerExists = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
                if (customerExists == null)
                    throw new RestException(HttpStatusCode.NotFound, new { message = "Customer has not been found." });

                var car = Mapping.Map<Command, Car>(request);

                await _unitOfWork.Cars.AddAsync(car);

                var success = await _unitOfWork.SaveAsync();
                if (success) return await Task.FromResult(Unit.Value);

                throw new Exception("Problem saving changes");
            }
        }


    }
}