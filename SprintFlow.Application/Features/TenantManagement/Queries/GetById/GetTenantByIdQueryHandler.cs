using AutoMapper;
using MediatR;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.TenantManagement.Queries.GetById
{
    public sealed class GetTenantByIdQueryHandler
        : IRequestHandler<GetTenantByIdQuery, Result<GetTenantResponse>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;

        public GetTenantByIdQueryHandler(ITenantRepository tenantRepository, IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
        }

        public async Task<Result<GetTenantResponse>> Handle(
            GetTenantByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId, cancellationToken);
            if (tenant == null)
            {
                return Result<GetTenantResponse>.Failure(CommonErrors.EntityNotFound);
            }

            var result = _mapper.Map<GetTenantResponse>(tenant);
            return Result<GetTenantResponse>.Success(result);
        }
    }
}
