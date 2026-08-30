using MediatR;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Projects.Commands.Delete
{
    public sealed class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProjectCommandHandler(
            IProjectRepository projectRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork
        )
        {
            _projectRepository = projectRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteProjectCommand command,
            CancellationToken cancellationToken
        )
        {
            var project = await _projectRepository.GetByIdAsync(
                command.ProjectId,
                cancellationToken
            );

            if (project is null)
            {
                return Result.Failure(ProjectErrors.ProjectNotFound);
            }

            project.IsDeleted = true;
            project.DeletedAt = DateTime.UtcNow;
            project.DeletedBy = _currentUserService.UserId;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
