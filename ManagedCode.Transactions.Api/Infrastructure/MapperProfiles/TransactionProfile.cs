using AutoMapper;
using ManagedCode.Transactions.Api.Models;
using ManagedCode.Transactions.Data.Entities;

namespace ManagedCode.Transactions.Api.Infrastructure.MapperProfiles;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<TransactionModel, TransactionEntity>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.TransactionId))
            .ReverseMap();
    }
}