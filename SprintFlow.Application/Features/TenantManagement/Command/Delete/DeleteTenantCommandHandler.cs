using MediatR;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.TenantManagement.Command.Delete
{
    public sealed class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand, Result>
    {
        private readonly ITenantRepository _tenantRepository;

        public DeleteTenantCommandHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Result> Handle(
            DeleteTenantCommand request,
            CancellationToken cancellationToken
        )
        {
            var result = await _tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);

            if (result is null)
            {
                return Result.Failure(CommonErrors.EntityNotFound);
            }

            await _tenantRepository.DeleteAsync(result, cancellationToken);

            return Result.Success();
        }
    }
}
