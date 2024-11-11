using AutoMapper;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Models.MessageModels;
using WebApp.Infrastructure.Models.ReportModels;
using WebApp.Infrastructure.Models.TopicModels;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.BusinessLogic;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        this.CreateMap<Message, MessageBriefModel>()
            .ForMember(dest => dest.SenderNickname, opt => opt.MapFrom(src => src.User.Nickname))
            .ForMember(dest => dest.RepliesCount, opt => opt.MapFrom(src => src.Replies.Count))
            .ForMember(dest => dest.ParentMessageId, opt => opt.MapFrom(src => src.ParentMessageId))
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.User.ProfilePictureUrl))
            .ReverseMap();

        this.CreateMap<Message, MessageModel>()
            .ForMember(dest => dest.ParentMessage, opt => opt.MapFrom(src => src.ParentMessage))
            .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies))
            .ForMember(dest => dest.Reports, opt => opt.MapFrom(src => src.Reports))
            .ReverseMap();

        this.CreateMap<Message, MessageCreateModel>()
            .ReverseMap();

        this.CreateMap<Message, MessageUpdateModel>()
            .ReverseMap();

        this.CreateMap<Report, ReportModel>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ReverseMap();

        this.CreateMap<Report, ReportSummaryModel>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message.Content))
            .ForMember(dest => dest.ReportedUser, opt => opt.MapFrom(src => src.Message.User.Nickname))
            .ForMember(dest => dest.Reporter, opt => opt.MapFrom(src => src.User.Nickname))
            .ReverseMap();

        this.CreateMap<Report, ReportCreateModel>()
            .ReverseMap();

        this.CreateMap<Report, ReportUpdateModel>()
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

        this.CreateMap<Topic, TopicDialogModel>()
            .ForMember(t => t.CreatorNickname, tm => tm.MapFrom(x => x.User.Nickname))
            .ForMember(t => t.Messages, opt => opt.MapFrom(src => src.Messages))
            .ReverseMap();

        this.CreateMap<AdminTopicCreateModel, Topic>()
            .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
            .ReverseMap();

        this.CreateMap<TopicCreateModel, Topic>()
            .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
            .ReverseMap();

        this.CreateMap<AdminTopicCreateModel, TopicCreateModel>()
            .ReverseMap();

        this.CreateMap<TopicUpdateModel, Topic>()
            .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null));

        this.CreateMap<User, UserModel>()
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages))
            .ForMember(dest => dest.Reports, opt => opt.MapFrom(src => src.Reports))
            .ForMember(dest => dest.OwnedTopics, opt => opt.MapFrom(src => src.Topics.Where(t => t.UserId == src.Id)))
            .ForMember(dest => dest.ParticipatedTopics, opt => opt.MapFrom(src => src.Topics.Where(t => t.UserId != src.Id)))
            .ReverseMap();

        this.CreateMap<User, UserPublicProfileModel>()
            .ReverseMap();

        this.CreateMap<User, UserRegisterModel>()
            .ForMember(dest => dest.Password, src => src.MapFrom(src => src.PasswordHash))
            .ReverseMap();

        this.CreateMap<UserUpdateModel, User>()
            .ForMember(dest => dest.Nickname, opt => opt.Condition(src => src.Nickname != null))
            .ForMember(dest => dest.Email, opt => opt.Condition(src => src.Email != null))
            .ReverseMap();

        this.CreateMap<User, UserLoginModel>()
            .ReverseMap();
    }
}
