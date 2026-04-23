using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Ingredients.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using ErrorOr;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Ingredients;

internal class IngredientsHandler : IIngredientsHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IValidator<CreateIngredientDto> _validator;
    private readonly IMapper _mapper;

    public IngredientsHandler(IApplicationDbContext dbContext, IValidator<CreateIngredientDto> validator,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<PaginatedData<IngredientDto>> GetPaginatedIngredients(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Ingredients
            .ProjectTo<IngredientDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(1, 10, cancellationToken);
    }

    public async Task<ErrorOr<int>> CreateIngredient(CreateIngredientDto dto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(dto);

        if (!validationResult.IsValid)
        {
            return Error.Validation(IngredientErrorCodes.Validation);
        }

        var ingredient = new Ingredient
        {
            Name = dto.Name,
            ProviderIngredients = dto.ProviderIds.Select(id => new IngredientProvider
            {
                ProviderId = id
            }).ToList()
        };

        await _dbContext.Ingredients.AddAsync(ingredient, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return ingredient.Id;
    }

    public async Task<ErrorOr<bool>> UpdateIngredient(UpdateIngredientDto dto,
        CancellationToken cancellationToken = default)
    {
        var ingredient = await _dbContext.Ingredients.FindAsync([dto.Id], cancellationToken);

        if (ingredient == null)
        {
            return Error.NotFound(IngredientErrorCodes.NotFound);
        }

        ingredient.Name = dto.Name;
        ingredient.ProviderIngredients = dto.ProviderIds.Select(id => new IngredientProvider
        {
            ProviderId = id
        }).ToList();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}