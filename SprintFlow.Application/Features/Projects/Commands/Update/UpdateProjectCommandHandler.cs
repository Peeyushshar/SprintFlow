using AutoMapper;
using MediatR;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;
using SprintFlow.Application.Common.MultiTenancy;

namespace SprintFlow.Application.Features.Projects.Update
{
    public class UpdateProjectCommandHandler
        : IRequestHandler<UpdateProjectCommand, Result<UpdateProjectResponse>>
    {
        private readonly IProjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentTenant _currentTenant;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectCommandHandler(
            IProjectRepository repository,
            IMapper mapper,
            ICurrentTenant currentTenant,
            ICurrentUserService userService,
            IUnitOfWork unitOfWork
        )
        {
            _repository = repository;
            _mapper = mapper;
            _currentTenant = currentTenant;
            _currentUserService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdateProjectResponse>> Handle(
            UpdateProjectCommand command,
            CancellationToken cancellationToken
        )
        {
            //-------------------------------------------------
            // Normalize project key
            //-------------------------------------------------

            var projectKey = command.Key.Trim().ToUpperInvariant();

            //-------------------------------------------------
            // Get existing project
            //-------------------------------------------------

            var project = await _repository.GetByIdAsync(command.ProjectId, cancellationToken);

            if (project is null)
            {
                return Result<UpdateProjectResponse>.Failure(ProjectErrors.ProjectNotFound);
            }

            var exists = await _repository.ExistsByKeyExceptAsync(
                projectKey,
                project.Id,
                cancellationToken
            );

            if (exists)
            {
                return Result<UpdateProjectResponse>.Failure(ProjectErrors.ProjectKeyAlreadyExists);
            }

            // Update allowed properties
            _mapper.Map(command, project);
            project.Key = projectKey;
            // Audit fields
            project.UpdatedAt = DateTime.UtcNow;
            project.UpdatedBy = _currentUserService.UserId;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<UpdateProjectResponse>(project);

            return Result<UpdateProjectResponse>.Success(response);
        }
    }
}
