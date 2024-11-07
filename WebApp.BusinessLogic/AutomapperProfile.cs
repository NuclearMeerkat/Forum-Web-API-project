using AutoMapper;
using WebApp.Core.Entities;
using WebApp.Core.Models;

namespace WebApp.BusinessLogic;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        this.CreateMap<Message, MessageBriefModel>()
            .ReverseMap();

        this.CreateMap<Message, MessageModel>()
            .ForMember(dest => dest.ParentMessage, opt => opt.MapFrom(src => src.ParentMessage))
            .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies))
            .ForMember(dest => dest.Reports, opt => opt.MapFrom(src => src.Reports))
            .ReverseMap();

        this.CreateMap<Message, MessageCreateModel>()
            .ReverseMap();

        this.CreateMap<Report, ReportModel>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ReverseMap();

        this.CreateMap<Report, ReportSummaryModel>()
            .ReverseMap();

        this.CreateMap<Report, ReportCreateModel>()
            .ReverseMap();

        this.CreateMap<Topic, TopicModel>()
            .ForMember(t => t.CreatorNickname, tm => tm.MapFrom(x => x.User.Nickname))
            .ForMember(t => t.CreatorEmail, tm => tm.MapFrom(x => x.User.Email))
            .ForMember(t => t.CreatorProfilePictureUrl, tm => tm.MapFrom(x => x.User.ProfilePictureUrl))
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages))
            .ReverseMap();

        this.CreateMap<Topic, TopicSummaryModel>()
            .ForMember(t => t.CreatorNickname, tm => tm.MapFrom(x => x.User.Nickname))
            .ReverseMap();

        this.CreateMap<Topic, TopicDtoModel>()
            .ReverseMap();

        this.CreateMap<User, UserModel>()
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages))
            .ForMember(dest => dest.Reports, opt => opt.MapFrom(src => src.Reports))
            .ForMember(dest => dest.OwnedTopics, opt => opt.MapFrom(src => src.Topics.Where(t => t.UserId == src.Id)))
            .ForMember(dest => dest.ParticipatedTopics, opt => opt.MapFrom(src => src.Topics.Where(t => t.UserId != src.Id)))
            .ReverseMap();

        this.CreateMap<User, UserPublicProfileModel>()
            .ReverseMap();

        this.CreateMap<User, UserCreateModel>()
            .ReverseMap();
    }
}
