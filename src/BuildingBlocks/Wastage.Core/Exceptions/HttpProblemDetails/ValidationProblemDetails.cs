using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wastage.Core.Exceptions.Type;

namespace Wastage.Core.Exceptions.HttpProblemDetails;

public class ValidationProblemDetails : ProblemDetails
{
    public IEnumerable<ValidationExceptionModel> Errors { get; set; }

    public ValidationProblemDetails(IEnumerable<ValidationExceptionModel> errors)
    {
        Title = "Validation error(s)";
        Detail = "One or more validation errors occurred";
        Errors = errors;
        Status = StatusCodes.Status422UnprocessableEntity;
        Type = "https://doc/props/validation";
    }
}
