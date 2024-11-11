// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0065:Misplaced using directive", Justification = "1", Scope = "namespace", Target = "~N:WebApp.Infrastructure")]
[assembly: SuppressMessage("Style", "IDE0065:Misplaced using directive", Justification = "1", Scope = "namespace", Target = "~N:WebApp.Infrastructure.Models.TopicModels")]
[assembly: SuppressMessage("Style", "IDE0065:Misplaced using directive", Justification = "1", Scope = "namespace", Target = "~N:WebApp.Infrastructure.Entities")]
[assembly: SuppressMessage("Style", "IDE0065:Misplaced using directive", Justification = "1", Scope = "namespace", Target = "~N:WebApp.Infrastructure.Models.UserModels")]
[assembly: SuppressMessage("Style", "IDE0065:Misplaced using directive", Justification = "1", Scope = "namespace", Target = "~N:WebApp.Infrastructure.Auth")]
[assembly: SuppressMessage("Style", "IDE0065:Misplaced using directive", Justification = "1", Scope = "namespace", Target = "~N:WebApp.Infrastructure.Interfaces.IRepositories")]
[assembly: SuppressMessage("Style", "IDE0065:Misplaced using directive", Justification = "1", Scope = "namespace", Target = "~N:WebApp.Infrastructure.Interfaces.Auth")]
[assembly: SuppressMessage("Style", "IDE0065:Misplaced using directive", Justification = "1", Scope = "namespace", Target = "~N:WebApp.Infrastructure.Interfaces.IServices")]
[assembly: SuppressMessage("Style", "IDE0065:Misplaced using directive", Justification = "1", Scope = "namespace", Target = "~N:WebApp.Infrastructure.Models.MessageModels")]
[assembly: SuppressMessage("Style", "IDE0065:Misplaced using directive", Justification = "1", Scope = "namespace", Target = "~N:WebApp.Infrastructure.Models.ReportModels")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<1>", Scope = "member", Target = "~M:WebApp.Infrastructure.Auth.JwtProvider.#ctor(Microsoft.Extensions.Options.IOptions{WebApp.Infrastructure.Auth.JwtOptions})")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<1>", Scope = "member", Target = "~M:WebApp.Infrastructure.Auth.JwtProvider.GenerateToken(WebApp.Infrastructure.Entities.User)~System.String")]
[assembly: SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "<1>", Scope = "member", Target = "~P:WebApp.Infrastructure.Entities.User.ProfilePictureUrl")]
[assembly: SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "<1>", Scope = "member", Target = "~P:WebApp.Infrastructure.Models.MessageModels.MessageBriefModel.ProfilePictureUrl")]
[assembly: SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "<1>", Scope = "member", Target = "~P:WebApp.Infrastructure.Models.TopicModels.TopicModel.CreatorProfilePictureUrl")]
[assembly: SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "<1>", Scope = "member", Target = "~P:WebApp.Infrastructure.Models.UserModels.UserModel.ProfilePictureUrl")]
[assembly: SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "<1>", Scope = "member", Target = "~P:WebApp.Infrastructure.Models.UserModels.UserPublicProfileModel.ProfilePictureUrl")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1000:Keywords should be spaced correctly", Justification = "<1>", Scope = "member", Target = "~M:WebApp.Infrastructure.Auth.JwtProvider.GenerateToken(WebApp.Infrastructure.Entities.User)~System.String")]
[assembly: SuppressMessage("Performance", "CA1805:Do not initialize unnecessarily", Justification = "<1>", Scope = "member", Target = "~P:WebApp.Infrastructure.Models.MessageModels.MessageCreateModel.ParentMessageId")]
[assembly: SuppressMessage("Minor Code Smell", "S1694:An abstract class should have both abstract and concrete methods", Justification = "<1>", Scope = "type", Target = "~T:WebApp.Infrastructure.Entities.BaseEntity")]
