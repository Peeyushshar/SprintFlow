namespace SprintFlow.Domain.Constants
{
    public static class Roles
    {
        public const string Owner = "Owner";
        public const string Admin = "Admin";
        public const string ProjectManager = "ProjectManager";
        public const string Developer = "Developer";
        public const string Viewer = "Viewer";

        public static readonly IReadOnlyList<string> All =
        [
            Owner,
            Admin,
            ProjectManager,
            Developer,
            Viewer,
        ];
    }
}
