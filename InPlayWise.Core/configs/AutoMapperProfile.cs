//using AutoMapper;
//using InPlayWise.Common.DTO;
//using InPlayWise.Data.Entities.SportsEntities;

//namespace InPlayWise.Core.configs
//{
//    public class AutoMapperProfile : Profile
//    {
//        public AutoMapperProfile()
//        {
//            CreateMap<RecentMatchModel, RecentMatchDto>()
//                .ForMember(dest => dest.HomeTeamName, opt => opt.MapFrom(src => src.HomeTeam.Name))
//                .ForMember(dest => dest.HomeTeamLogo, opt => opt.MapFrom(src => src.HomeTeam.Logo))
//                .ForMember(dest => dest.AwayTeamName, opt => opt.MapFrom(src => src.AwayTeam.Name))
//                .ForMember(dest => dest.AwayTeamLogo, opt => opt.MapFrom(src => src.AwayTeam.Logo))
//                .ForMember(dest => dest.CompetitionName, opt => opt.MapFrom(src => src.Competition.Name))
//                .ForMember(dest => dest.CompetitionLogo, opt => opt.MapFrom(src => src.Competition.Logo));
//            CreateMap<RecentMatchDto, RecentMatchModel>();
//        }
//    }
//}
