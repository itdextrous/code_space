using AutoMapper;

namespace SAASCLOUDAPP.BusinessLayer
{
    public static class AutoMapperConfig
    {
        public static IMapper Build()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            //config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }
    }
}
