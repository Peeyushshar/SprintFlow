using AutoMapper;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.CQRS;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;
using SprintFlow.Application.Common.MultiTenancy;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Features.Projects.Commands.Create
{
    public class CreateProjectCommandHandler
        : ICommandHandler<CreateProjectCommand, Result<CreateProjectResponse>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICurrentTenant _currentTenant;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectCommandHandler(
            IProjectRepository projectRepository,
            ICurrentUserService currentUserService,
            ICurrentTenant currentTenant,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _projectRepository = projectRepository;
            _currentUserService = currentUserService;
            _currentTenant = currentTenant;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateProjectResponse>> Handle(
            CreateProjectCommand request,
            CancellationToken cancellationToken
        )
        {
            var projectKey = request.Key.Trim().ToUpperInvariant();

            var exists = await _projectRepository.ExistsByKeyAsync(projectKey, cancellationToken);

            if (exists)
            {
                return Result<CreateProjectResponse>.Failure(ProjectErrors.ProjectKeyAlreadyExists);
            }

            var project = new Project
            {
                Name = request.Name.Trim(),
                Key = projectKey,
                Description = request.Description?.Trim(),
                IsActive = true,
                CreatedBy = _currentUserService.UserId,
            };

            await _projectRepository.AddAsync(project, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CreateProjectResponse>(project);

            return Result<CreateProjectResponse>.Success(response);
        }
    }
}
