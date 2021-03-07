using AWS.Insurance.Operations.Application.Cache;
using AWS.Insurance.Operations.Application.Customers.Dtos;
using AWS.Insurance.Operations.Application.Errors;
using AWS.Insurance.Operations.Application.Gateways;
using AWS.Insurance.Operations.Application.Map;
using AWS.Insurance.Operations.Data.UoW;
using AWS.Insurance.Operations.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Application.Customers
{
    public class Create
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public int CNumber { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime Dob { get; set; }
            public int ZipCode { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Age).NotEmpty();
                RuleFor(x => x.Dob).NotEmpty();
                RuleFor(x => x.CNumber).NotEmpty();
                RuleFor(x => x.ZipCode).InclusiveBetween(1, 9999);
            }
        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IDistributedCache _cache;
            private readonly ILocationService _locationService;

            public Handler(IUnitOfWork unitOfWork,
                           IDistributedCache cache,
                            ILocationService locationService)
            {
                _unitOfWork = unitOfWork;
                _cache = cache;
                _locationService = locationService;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var customerExists = await _unitOfWork.Customers.GetByCNumberAsync(request.CNumber);
                if (customerExists != null)
                    throw new RestException(HttpStatusCode.BadRequest, new { message = "Cnumber already taken." });

                var customer = Mapping.Map<Command, Customer>(request);

                var cacheCustomerZone = await _cache.GetAsync<LocationDto>($"{customer.ZipCode}");
                if (cacheCustomerZone == null)
                {
                    cacheCustomerZone = await _locationService.GetLocation(customer.ZipCode);
                    await _cache.SetAsync($"{customer.ZipCode}", cacheCustomerZone,
                                                        new DistributedCacheEntryOptions
                                                        {
                                                            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                                                        });
                }
                customer.AddZone(cacheCustomerZone.Zone);

                await _unitOfWork.Customers.AddAsync(customer);
                var success = await _unitOfWork.SaveAsync();
                if (success) return await Task.FromResult(Unit.Value);

                throw new Exception("Problem saving changes");
            }
        }
    }
}