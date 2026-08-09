using SprintFlow.Application.Enums;

namespace SprintFlow.Application.Common.Models
{
    public sealed record Error(
        string Code,
        string Message,
        ErrorType Type)
    {
        public static readonly Error None =
            new("", "", ErrorType.Failure);
    }
}
