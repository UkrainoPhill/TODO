namespace TODO.API.Contracts.UserContracts;

public record UserLoginInputDto(string emailOrUsername, string password);