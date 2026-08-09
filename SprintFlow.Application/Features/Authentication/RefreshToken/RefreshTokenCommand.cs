using MediatR;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Authentication.RefreshToken
{
    public class RefreshTokenCommand : IRequest<Result<RefreshTokenResponse>>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
