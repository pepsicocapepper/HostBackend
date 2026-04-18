using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Ingredients.Dtos;
using Application.Recipes.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Recipes;

internal class RecipesHandler : IRecipesHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public RecipesHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedData<RecipeDto>> GetPaginatedRecipes(PaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Recipes
            .ProjectTo<RecipeDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(query, cancellationToken);
    }

    public async Task<ErrorOr<PaginatedData<IngredientDto>>> GetPaginatedIngredientsNotInRecipe(
        PaginationQuery paginationQuery,
        Guid recipeId,
        CancellationToken cancellationToken = default)
    {
        var recipe = await _dbContext
            .Recipes
            .AnyAsync(b => b.Id == recipeId, cancellationToken);

        if (!recipe)
        {
            return Error.NotFound(RecipeErrorCodes.NotFound);
        }

        return await _dbContext
            .Ingredients
            .Where(t => !_dbContext.RecipeIngredients.Any(ri => ri.IngredientId == t.Id && ri.RecipeId == recipeId))
            .ProjectTo<IngredientDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(paginationQuery, cancellationToken);
    }

    public async Task<ErrorOr<int>> CreateRecipe(CreateRecipeDto dto, CancellationToken cancellationToken = default)
    {
        var recipe = new Recipe
        {
            Name = dto.Name,
            RecipeIngredients = dto.Ingredients.Select(s => new RecipeIngredient
            {
                IngredientId = s.IngredientId,
                Unit = s.Unit,
                Quantity = s.Quantity
            }).ToList(),
            Steps = dto.Steps,
        };

        await _dbContext.Recipes.AddAsync(recipe, cancellationToken);
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}