using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common.Extensions;

public static class ErrorExtensions
{
    public static ProblemDetails ToProblemDetails(this Error error)
    {
        return new ProblemDetails
        {
            Title = error.Code,
            Detail = error.Description,
        };
    }
}