using AWS.Insurance.STS.Application.App.Dtos;
using AWS.Insurance.STS.Application.Errors;
using AWS.Insurance.STS.Application.Gateways;
using FluentValidation;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AWS.Insurance.STS.Application.App
{
    public class Auth
    {
        public class Query : IRequest<TokenDto>
        {
            public string AppClient { get; set; }
            public string AppSecret { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.AppClient).NotEmpty();
                RuleFor(x => x.AppSecret).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, TokenDto>
        {
            private readonly IAppAccessor _appAccessor;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(IAppAccessor appAccessor,
                           IJwtGenerator jwtGenerator)
            {
                _appAccessor = appAccessor;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<TokenDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var appAuthOK = await _appAccessor.ValidateAppCredentials(request.AppClient, request.AppSecret);
                if (appAuthOK)
                {
                    return new TokenDto
                    {
                        Token = _jwtGenerator.CreateToken()
                    };
                }

                throw new RestException(HttpStatusCode.Unauthorized);
            }
        }
    }
}
