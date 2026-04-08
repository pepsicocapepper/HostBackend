namespace Application.Menus.Dtos;

public record CreateMenuDto(
    string Name,
    string? PosName,
    int? ParentId
);