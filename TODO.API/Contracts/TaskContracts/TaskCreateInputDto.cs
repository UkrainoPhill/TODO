using System.Runtime.InteropServices;
using TODO.Core.Enum;

namespace TODO.API.Contracts.TaskContracts;

public record TaskCreateInputDto(
    string title,
    DateTime dueDate,
    int status,
    int priority,
    string? description = null);