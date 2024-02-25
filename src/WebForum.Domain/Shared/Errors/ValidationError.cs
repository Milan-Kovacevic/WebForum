namespace WebForum.Domain.Shared.Errors;

public record ValidationError(PropertyError[] Errors) 
    : Error("Error.Validation", "A validation problem occured.", 400);

public record PropertyError(string Property, string Message);