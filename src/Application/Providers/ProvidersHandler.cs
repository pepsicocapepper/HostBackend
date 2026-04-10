using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Ingredients.Dtos;
using Application.Providers.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using ErrorOr;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Providers;

public class ProvidersHandler : IProvidersHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateProviderDto> _createProviderDtoValidator;

    public ProvidersHandler(IApplicationDbContext dbContext, IMapper mapper,
        IValidator<CreateProviderDto> createProviderDtoValidator)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _createProviderDtoValidator = createProviderDtoValidator;
    }

    public async Task<ErrorOr<Guid>> CreateProvider(CreateProviderDto dto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _createProviderDtoValidator.Validate(dto);

        if (!validationResult.IsValid)
        {
            return Error.Validation(ProviderErrorCodes.ValidationError);
        }

        var provider = new Provider
        {
            Name = dto.Name,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            Address = dto.Address
        };

        await _dbContext.Providers.AddAsync(provider, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return provider.Id;
    }

    public async Task<PaginatedData<ProviderDto>> GetPaginatedProviders(PaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Providers
            .ProjectTo<ProviderDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(query, cancellationToken);
    }

    public async Task<ErrorOr<ProviderDto>> GetProviderById(Guid id, CancellationToken cancellationToken = default)
    {
        var provider = await _dbContext.Providers.FindAsync(id, cancellationToken);

        if (provider is null)
        {
            return Error.NotFound(ProviderErrorCodes.NotFound);
        }

        return _mapper.Map<ProviderDto>(provider);
    }

    public async Task<PaginatedData<IngredientProviderDto>> GetPaginatedProviderIngredients(
        PaginationQuery query,
        Guid providerId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .IngredientProviders
            .Where(t => t.ProviderId == providerId)
            .Include(t => t.Ingredient)
            .ProjectTo<IngredientProviderDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(query, cancellationToken);
    }

    public async Task<ErrorOr<bool>> AddIngredient(CreateIngredientProviderDto dto,
        CancellationToken cancellationToken = default)
    {
        var providerResult = await _dbContext
            .Providers
            .FindAsync(dto.ProviderId, cancellationToken);

        var ingredientResult = await _dbContext
            .Ingredients
            .FindAsync(dto.IngredientId, cancellationToken);


        if (providerResult is null)
        {
            return Error.NotFound(ProviderErrorCodes.NotFound);
        }

        if (ingredientResult is null)
        {
            return Error.NotFound(ProviderErrorCodes.IngredientNotFound);
        }

        var ingredientProvider = new IngredientProvider
        {
            ProviderId = dto.ProviderId,
            IngredientId = dto.IngredientId
        };

        await _dbContext.IngredientProviders.AddAsync(ingredientProvider, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}