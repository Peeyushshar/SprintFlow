namespace SprintFlow.Application.Features.Projects.Queries.GetProject
{
    public class GetProjectsResponse
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = null!;

        public string Key { get; init; } = null!;

        public string? Description { get; init; }

        public bool IsActive { get; init; }

        public DateTime CreatedAt { get; init; }
    }
}
