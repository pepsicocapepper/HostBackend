using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Models;

public record PaginationQuery(int? PageNumber, int? PageSize);