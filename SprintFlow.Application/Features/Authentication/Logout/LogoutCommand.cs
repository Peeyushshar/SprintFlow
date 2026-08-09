using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Authentication.Logout
{
    public record LogoutCommand(string RefreshToken) : IRequest<Result>;
}
