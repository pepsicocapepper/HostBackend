using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Ingredients.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using ErrorOr;
using FluentValidation;

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
            return Error.Validation(IngredientErrorCodes.ValidationError);
        }

        var ingredient = new Ingredient
        {
            Name = dto.Name,
        };

        await _dbContext.Ingredients.AddAsync(ingredient, cancellationToken);

        if (dto.ProviderIds.Count > 0)
        {
            foreach (var provider in dto.ProviderIds)
            {
                await _dbContext.IngredientProviders.AddAsync(new IngredientProvider
                {
                    Ingredient = ingredient,
                    ProviderId = provider,
                }, cancellationToken);
            }
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return ingredient.Id;
    }
}