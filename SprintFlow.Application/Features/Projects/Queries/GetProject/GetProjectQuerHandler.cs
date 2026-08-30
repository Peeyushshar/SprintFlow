using AutoMapper;
using MediatR;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Projects.Queries.GetProject
{
    public sealed class GetProjectsQueryHandler
        : IRequestHandler<GetProjectsQuery, Result<IReadOnlyList<GetProjectsResponse>>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetProjectsQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<GetProjectsResponse>>> Handle(
            GetProjectsQuery request,
            CancellationToken cancellationToken
        )
        {
            var projects = await _projectRepository.GetAllAsync(cancellationToken);

            var response = _mapper.Map<IReadOnlyList<GetProjectsResponse>>(projects);

            return Result<IReadOnlyList<GetProjectsResponse>>.Success(response);
        }
    }
}
