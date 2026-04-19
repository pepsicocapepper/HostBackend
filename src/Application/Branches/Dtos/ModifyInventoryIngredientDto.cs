using Domain.Common;

namespace Application.Branches.Dtos;

public record ModifyInventoryIngredientDto(int Id, decimal Quantity, BranchInventoryAction Action);