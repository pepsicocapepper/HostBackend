using Microsoft.EntityFrameworkCore;
namespace Application.Common.Models;
public class PaginatedData<T>
{
    public IReadOnlyList<T> Data { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public PaginatedData(IReadOnlyList<T> data, int pageNumber, int pageSize, int totalCount)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public bool HasPreviousPage => PageNumber > 1 && PageNumber <= TotalPages;
    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedData<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return new PaginatedData<T>(items, pageNumber, pageSize, count);
    }
}