using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wastage.Core.Exceptions.HttpProblemDetails;

public class BusinessProblemDetails : ProblemDetails
{
    public BusinessProblemDetails(string detail)
    {
        Title = "Business Error";
        Detail = detail;
        Status = StatusCodes.Status400BadRequest;
        Type = "https://example.com/props/business";
    }

}
