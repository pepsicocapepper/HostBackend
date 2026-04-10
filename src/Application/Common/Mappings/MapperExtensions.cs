using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Mappings;

public static class MapperExtensions
{
    public static Task<PaginatedData<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, int pageNumber, int pageSize,
        CancellationToken cancellationToken = default) where TDestination : class
        => PaginatedData<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize, cancellationToken);

    public static Task<PaginatedData<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, PaginationQuery query,
        CancellationToken cancellationToken = default) where TDestination : class
        => PaginatedData<TDestination>.CreateAsync(queryable.AsNoTracking(),
            query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable<TDestination> queryable,
        IConfigurationProvider configurationProvider, CancellationToken cancellationToken = default)
        where TDestination : class
        => queryable.ProjectTo<TDestination>(configurationProvider).AsNoTracking().ToListAsync(cancellationToken);
}