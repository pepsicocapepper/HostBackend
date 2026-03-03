using Application.Common.Interfaces;

namespace Api.Services;

public class CurrentLocation : ILocation
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentLocation(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Id
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.Request.Headers["Location-Id"];
            if (string.IsNullOrEmpty(id))
            {
                throw new BadHttpRequestException("No Location-Id header found");
            }

            return id;
        }
    }
}