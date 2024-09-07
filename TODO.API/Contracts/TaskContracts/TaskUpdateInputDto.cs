using TODO.Core.Enum;

namespace TODO.API.Contracts.TaskContracts;

public record TaskUpdateInputDto( 
    string Title, 
    string Description, 
    DateTime DueDate, 
    int Status,
    int Priority);