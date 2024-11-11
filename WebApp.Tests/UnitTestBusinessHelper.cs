using AutoMapper;
using WebApp.BusinessLogic;

namespace WebApp.Tests;

public static class UnitTestBusinessHelper
{
    public static IMapper CreateMapperProfile()
    {
        var myProfile = new AutomapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

        return new Mapper(configuration);
    }
}
