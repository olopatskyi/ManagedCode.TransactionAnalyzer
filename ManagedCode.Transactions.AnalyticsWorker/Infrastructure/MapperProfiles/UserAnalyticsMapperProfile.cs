using AutoMapper;
using ManagedCode.Transactions.AnalyticsWorker.Models;
using ManagedCode.Transactions.Data.Entities;

namespace ManagedCode.Transactions.AnalyticsWorker.Infrastructure.MapperProfiles;

public class UserAnalyticsMapperProfile : Profile
{
    public UserAnalyticsMapperProfile()
    {
        CreateMap<UserAnalyticsModel, UserAnalyticsEntity>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.UserId));
    }
}