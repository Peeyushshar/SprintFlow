using AutoMapper;
using MediatR;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.TenantManagement.Queries.GetAll
{
    public sealed class GetTenantQueryHandler
        : IRequestHandler<GetTenantQuery, Result<IReadOnlyList<GetTenantResponse>>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;

        public GetTenantQueryHandler(ITenantRepository tenantRepository, IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<GetTenantResponse>>> Handle(
            GetTenantQuery tenantQuery,
            CancellationToken cancellationToken
        )
        {
            var tenants = await _tenantRepository.GetAllTenantsAsync(cancellationToken);

            var response = _mapper.Map<IReadOnlyList<GetTenantResponse>>(tenants);

            return Result<IReadOnlyList<GetTenantResponse>>.Success(response);
        }
    }
}
