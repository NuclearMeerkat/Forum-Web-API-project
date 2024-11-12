// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "1", Scope = "member", Target = "~M:WebApp.BusinessLogic.AutomapperProfile.#ctor")]
[assembly: SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "1", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.ThrowIfMessageCreateModelIsNotCorrect(WebApp.Infrastructure.Models.MessageModels.MessageCreateModel,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "1", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.ThrowIfMessageBriefModelIsNotCorrect(WebApp.Infrastructure.Models.MessageModels.MessageBriefModel,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "1", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.ThrowIfReportSummaryModelIsNotCorrect(WebApp.Infrastructure.Models.ReportModels.ReportSummaryModel,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "1", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.ThrowIfTopicModelIsNotCorrect(WebApp.Infrastructure.Models.TopicModels.TopicModel,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "1", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.ThrowIfReportModelIsNotCorrect(WebApp.Infrastructure.Models.ReportModels.ReportModel,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "1", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.ThrowIfReportCreateModelIsNotCorrect(WebApp.Infrastructure.Models.ReportModels.ReportCreateModel,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "1", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.ThrowIfMessageModelIsNotCorrect(WebApp.Infrastructure.Models.MessageModels.MessageModel,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "1", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.ThrowIfTopicCreateModelIsNotCorrect(WebApp.Infrastructure.Models.TopicModels.AdminTopicCreateModel,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "1", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.ThrowIfTopicSummaryModelIsNotCorrect(WebApp.Infrastructure.Models.TopicModels.TopicSummaryModel,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "1", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.ThrowIfUserCreateModelIsNotCorrect(WebApp.Infrastructure.Models.UserModels.UserRegisterModel,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.ThrowIfUserModelIsNotCorrect(WebApp.Infrastructure.Models.UserModels.UserModel,System.String)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.ReportService.DeleteAsync(WebApp.Infrastructure.Entities.CompositeKey)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.TopicService.GetByIdAsync(System.Object[])~System.Threading.Tasks.Task{WebApp.Infrastructure.Models.TopicModels.TopicSummaryModel}")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.TopicService.GetByIdWithDetailsAsync(System.Object[])~System.Threading.Tasks.Task{WebApp.Infrastructure.Models.TopicModels.TopicDialogModel}")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.UserService.LoginAsync(WebApp.Infrastructure.Models.UserModels.UserLoginModel)~System.Threading.Tasks.Task{System.String}")]
[assembly: SuppressMessage("Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.TopicService.GetAllAsync(WebApp.Infrastructure.Models.TopicModels.TopicQueryParametersModel)~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{WebApp.Infrastructure.Models.TopicModels.TopicSummaryModel}}")]
[assembly: SuppressMessage("Critical Code Smell", "S1006:Method overrides should not change parameter defaults", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.MessageService.GetAllAsync(WebApp.Infrastructure.Models.TopicModels.TopicQueryParametersModel)~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{WebApp.Infrastructure.Models.MessageModels.MessageBriefModel}}")]
[assembly: SuppressMessage("Major Code Smell", "S112:General or reserved exceptions should never be thrown", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.ReportService.UpdateAsync(WebApp.Infrastructure.Models.ReportModels.ReportUpdateModel)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Major Code Smell", "S6561:Avoid using \"DateTime.Now\" for benchmarking or timing operations", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.TopicService.CalculateActivity(System.Collections.Generic.ICollection{WebApp.Infrastructure.Models.MessageModels.MessageModel})~System.Double")]
[assembly: SuppressMessage("Major Code Smell", "S1696:NullReferenceException should not be caught", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.TopicService.RateTopic(System.Int32,System.Int32,System.Int32)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.ReportService.UpdateAsync(WebApp.Infrastructure.Models.ReportModels.ReportUpdateModel)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.TopicService.#ctor(WebApp.Infrastructure.Interfaces.IRepositories.IUnitOfWork,AutoMapper.IMapper)")]
[assembly: SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.ReportService.UpdateAsync(WebApp.Infrastructure.Models.ReportModels.ReportUpdateModel)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Validation.ForumException.Throw(System.String)")]
[assembly: SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<1>", Scope = "type", Target = "~T:WebApp.BusinessLogic.Validation.ForumException")]
[assembly: SuppressMessage("Major Code Smell", "S1696:NullReferenceException should not be caught", Justification = "<1>", Scope = "member", Target = "~M:WebApp.BusinessLogic.Services.TopicService.RateTopicAsync(System.Int32,System.Int32,System.Int32)~System.Threading.Tasks.Task")]
