using AutoMapper;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.CQRS;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.TenantManagement.Command.Update
{
    public class UpdateTenantCommandHandler
        : ICommandHandler<UpdateTenantCommand, Result<UpdateTenantResponse>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;

        public UpdateTenantCommandHandler(ITenantRepository tenantRepository, IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
        }

        public async Task<Result<UpdateTenantResponse>> Handle(
            UpdateTenantCommand request,
            CancellationToken cancellationToken
        )
        {
            //-------------------------------------------------
            // Get existing tenant
            //-------------------------------------------------

            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);

            if (tenant is null)
            {
                return Result<UpdateTenantResponse>.Failure(CommonErrors.EntityNotFound);
            }

            //-------------------------------------------------
            // Update allowed properties
            //-------------------------------------------------

            tenant.Name = request.Name.Trim();
            tenant.SubscriptionPlan = request.SubscriptionPlan.Trim();

            //-------------------------------------------------
            // Repository
            //-------------------------------------------------

            await _tenantRepository.UpdateAsync(tenant, cancellationToken);

            //-------------------------------------------------
            // Response
            //-------------------------------------------------

            var response = _mapper.Map<UpdateTenantResponse>(tenant);

            return Result<UpdateTenantResponse>.Success(response);
        }
    }
}
