using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wastage.Core.Exceptions.HttpProblemDetails;

public class NotFoundProblemDetails : ProblemDetails
{
    public NotFoundProblemDetails(string detail)
    {
        Title = "Not Found";
        Detail = detail;
        Status = StatusCodes.Status404NotFound;
        Type = "https://example.com/props/business";
    }
}
