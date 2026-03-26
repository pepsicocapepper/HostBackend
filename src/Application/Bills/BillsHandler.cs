using Application.Bills.Dtos;
using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Bills;

internal class BillsHandler : IBillsHandler
{
    private readonly IUserContext _userContext;
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public BillsHandler(IUserContext userContext, IApplicationDbContext dbContext, IMapper mapper)
    {
        _userContext = userContext;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedData<BillDto>> GetPaginatedBillsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Bills
            .Include(t => t.CreatedByUser)
            .ProjectTo<BillDto>(_mapper.ConfigurationProvider, cancellationToken)
            .PaginatedListAsync(1, 10, cancellationToken);
    }

    public async Task<Guid> CreateBill(CreateBillDto createBillDto, CancellationToken cancellationToken = default)
    {
        var userId = _userContext.UserId;
        if (userId == null)
        {
            throw new ArgumentException("User not logged in");
        }

        var bill = new Bill
        {
            Amount = createBillDto.Amount,
            CreatedAt = createBillDto.CreatedAt,
            CreatedBy = _userContext.UserId!.Value,
            BillItems = createBillDto.BillItems.Select(x => new BillItem
                {
                    ItemId = x.ItemId,
                    Price = x.Price,
                    Denomination = x.Denomination,
                    Quantity = x.Quantity,
                    BillItemModifiers = x.Modifiers.Select(m => new BillItemModifier
                        {
                            ModifierElementId = m.ModifierId,
                            Price = m.Price,
                            Denomination = m.Denomination
                        })
                        .ToList()
                })
                .ToList()
        };

        await _dbContext.Bills.AddAsync(bill, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return bill.Id;
    }
}