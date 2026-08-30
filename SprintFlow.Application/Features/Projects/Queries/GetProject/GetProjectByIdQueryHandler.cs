using AutoMapper;
using MediatR;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Projects.Queries.GetProject
{
    public sealed class GetProjectByIdQueryHandler
        : IRequestHandler<GetProjectByIdQuery, Result<GetProjectsResponse>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetProjectByIdQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<Result<GetProjectsResponse>> Handle(
            GetProjectByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var project = await _projectRepository.GetByIdAsync(
                request.ProjectId,
                cancellationToken
            );

            if (project is null)
            {
                return Result<GetProjectsResponse>.Failure(ProjectErrors.ProjectNotFound);
            }

            var response = _mapper.Map<GetProjectsResponse>(project);

            return Result<GetProjectsResponse>.Success(response);
        }
    }
}
