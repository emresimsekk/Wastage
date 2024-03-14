namespace Wastage.Core.Exceptions.Type;

public class ValidationException : ApplicationException
{
    public IEnumerable<ValidationExceptionModel> Errors { get; }
    public ValidationException() : base("Validation Error(s)", "One or more validation errors occurred")
    {
        Errors = Array.Empty<ValidationExceptionModel>();
    }
    public ValidationException(string message) : base("Validation Error(s)", message)
    {
        Errors = Array.Empty<ValidationExceptionModel>();
    }
    public ValidationException(string message, Exception? innerException) : base("Validation Error(s)", message, innerException)
    {
        Errors = Array.Empty<ValidationExceptionModel>();
    }
    public ValidationException(IEnumerable<ValidationExceptionModel> errors) : base("Validation Error(s)", BuildErrorMessage(errors))
    {
        Errors = errors;
    }
    private static string BuildErrorMessage(IEnumerable<ValidationExceptionModel> errors)
    {
        IEnumerable<string> arr = errors.Select(
            x => $"{Environment.NewLine} -- {x.Property}: {string.Join(Environment.NewLine, values: x.Errors ?? Array.Empty<string>())}"
        );
        return $"Validation failed: {string.Join(string.Empty, arr)}";
    }
}
public class ValidationExceptionModel
{
    public string? Property { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}