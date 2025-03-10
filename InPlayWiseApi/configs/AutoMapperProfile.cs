using AutoMapper;
using InPlayWise.Common.DTO;
using InPlayWise.Common.DTO.CachedDto;
using InPlayWise.Data.Entities;
using InPlayWise.Data.Entities.SportsEntities;
using InPlayWise.Data.SportsEntities;

namespace InPlayWiseApi.configs
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RecentMatchModel, RecentMatchDto>()
                .ForMember(dest => dest.HomeTeamName, opt => opt.MapFrom(src => src.HomeTeam.Name))
                .ForMember(dest => dest.HomeTeamLogo, opt => opt.MapFrom(src => src.HomeTeam.Logo))
                .ForMember(dest => dest.AwayTeamName, opt => opt.MapFrom(src => src.AwayTeam.Name))
                .ForMember(dest => dest.AwayTeamLogo, opt => opt.MapFrom(src => src.AwayTeam.Logo))
                .ForMember(dest => dest.CompetitionName, opt => opt.MapFrom(src => src.Competition.Name))
                .ForMember(dest => dest.CompetitionLogo, opt => opt.MapFrom(src => src.Competition.Logo));
            CreateMap<RecentMatchDto, RecentMatchModel>();

            CreateMap<LiveMatchModel, LiveMatchDto>()
                .ForMember(dest => dest.HomeTeamName, opt => opt.MapFrom(src => src.HomeTeam.Name))
                .ForMember(dest => dest.HomeTeamLogo, opt => opt.MapFrom(src => src.HomeTeam.Logo))
                .ForMember(dest => dest.AwayTeamName, opt => opt.MapFrom(src => src.AwayTeam.Name))
                .ForMember(dest => dest.AwayTeamLogo, opt => opt.MapFrom(src => src.AwayTeam.Logo))
                .ForMember(dest => dest.CompetitionName, opt => opt.MapFrom(src => src.Competition.Name))
                .ForMember(dest => dest.CompetitionLogo, opt => opt.MapFrom(src => src.Competition.Logo))
                .ForMember(dest => dest.CompetitionType, opt => opt.MapFrom(src => src.CompetitionType));

            CreateMap<LiveMatchDto, LiveMatchModel>();
            CreateMap<Accumulator, OpportunityCachedDto>();
            CreateMap<OpportunityCachedDto, Accumulator>();
        }
    }
}
