using TODO.Core.Enum;

namespace TODO.API.Contracts.TaskContracts;

public record TaskGetOutputDto(Guid guid,
    string title, 
    string description, 
    DateTime dueDate, 
    Status status, 
    Priority priority,
    DateTime createdAt,
    DateTime updatedAt);