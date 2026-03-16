namespace Api.Common.Extensions;

public static class EndpointExtensions
{
    public static RouteHandlerBuilder RequireHeader(
        this RouteHandlerBuilder builder,
        string headerName
    )
    {
        return builder.AddEndpointFilter(async (context, next) =>
            {
                var request = context.HttpContext.Request;
                if (!request.Headers.TryGetValue(headerName, out var values))
                {
                    return Results.BadRequest($"Missing header '{headerName}'");
                }

                return await next(context);
            }
        );
    }
}