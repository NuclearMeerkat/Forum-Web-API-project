using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Models.MessageModels;
using WebApp.Infrastructure.Models.ReportModels;
using WebApp.Infrastructure.Models.TopicModels;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.BusinessLogic.Validation
{
    public class ForumException : Exception
    {
        public ForumException(string message)
            : base(message)
        {
        }

        public ForumException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ForumException()
        {
        }

#nullable enable
        public static void ThrowIfNull([NotNull] object? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfMessageCreateModelIsNotCorrect([NotNull] MessageCreateModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (int.IsNegative(argument.UserId) || argument.UserId == 0 ||
                int.IsNegative(argument.TopicId) || argument.TopicId == 0 ||
                string.IsNullOrEmpty(argument.Content) ||
                argument.CreatedAt < new DateTime(2024, 3, 10))
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfMessageBriefModelIsNotCorrect([NotNull] MessageBriefModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (int.IsNegative(argument.TopicId) || argument.TopicId == 0 ||
                string.IsNullOrEmpty(argument.Content) ||
                argument.CreatedAt < new DateTime(2024, 3, 10))
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfMessageModelIsNotCorrect([NotNull] MessageModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (int.IsNegative(argument.UserId) || argument.UserId == 0 ||
                int.IsNegative(argument.TopicId) || argument.TopicId == 0 ||
                string.IsNullOrEmpty(argument.Content) ||
                int.IsNegative(argument.LikesCounter) ||
                argument.CreatedAt < new DateTime(2024, 3, 10))
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfReportCreateModelIsNotCorrect([NotNull] ReportCreateModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (int.IsNegative(argument.UserId) || argument.UserId == 0 ||
                int.IsNegative(argument.MessageId) || argument.MessageId == 0 ||
                string.IsNullOrEmpty(argument.Reason) ||
                argument.CreatedAt < new DateTime(2024, 3, 10) ||
                int.IsNegative(argument.UserId) ||
                int.IsNegative(argument.MessageId))
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfReportSummaryModelIsNotCorrect([NotNull] ReportSummaryModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (string.IsNullOrEmpty(argument.Reason) ||
                argument.CreatedAt < new DateTime(2024, 3, 10) ||
                int.IsNegative(argument.MessageId) || argument.MessageId == 0 ||
                !Enum.IsDefined(typeof(ReportStatus), argument.Status))
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfReportModelIsNotCorrect([NotNull] ReportModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (string.IsNullOrEmpty(argument.Reason) ||
                argument.CreatedAt < new DateTime(2024, 3, 10) ||
                int.IsNegative(argument.UserId) || argument.UserId == 0 ||
                int.IsNegative(argument.MessageId) || argument.MessageId == 0 ||
                !Enum.IsDefined(typeof(ReportStatus), argument.Status))
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfTopicCreateModelIsNotCorrect([NotNull] AdminTopicCreateModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (string.IsNullOrEmpty(argument.Title) ||
                string.IsNullOrEmpty(argument.Description) ||
                argument.CreatedAt < new DateTime(2024, 3, 10) ||
                int.IsNegative(argument.UserId) || argument.UserId == 0)
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfTopicModelIsNotCorrect([NotNull] TopicModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (string.IsNullOrEmpty(argument.Title) ||
                string.IsNullOrEmpty(argument.Description) ||
                string.IsNullOrEmpty(argument.CreatorNickname) ||
                string.IsNullOrEmpty(argument.CreatorEmail) ||
                argument.CreatedAt < new DateTime(2024, 3, 10) ||
                int.IsNegative(argument.UserId) || argument.UserId == 0 ||
                double.IsNegative(argument.AverageStars))
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfTopicSummaryModelIsNotCorrect([NotNull] TopicSummaryModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (string.IsNullOrEmpty(argument.Title) ||
                string.IsNullOrEmpty(argument.CreatorNickname) ||
                argument.CreatedAt < new DateTime(2024, 3, 10) ||
                double.IsNegative(argument.AverageStars) ||
                double.IsNegative(argument.ActivityScore))
            {
                Throw(paramName);
            }
        }


        public static void ThrowIfUserCreateModelIsNotCorrect([NotNull] UserRegisterModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (string.IsNullOrEmpty(argument.Nickname) ||
                string.IsNullOrEmpty(argument.Email) ||
                !Enum.IsDefined(typeof(UserRole), argument.Role) ||
                argument.CreatedAt < new DateTime(2024, 3, 10))
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfUserModelIsNotCorrect([NotNull] UserModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (string.IsNullOrEmpty(argument.Nickname) ||
                string.IsNullOrEmpty(argument.Email) ||
                !Enum.IsDefined(typeof(UserRole), argument.Role) ||
                argument.CreatedAt < new DateTime(2024, 3, 10))
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfUserPublicProfileModelIsNotCorrect([NotNull] UserPublicProfileModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (string.IsNullOrEmpty(argument.Nickname))
            {
                Throw(paramName);
            }
        }

        [DoesNotReturn]
        internal static void Throw(string? paramName) =>
            throw new ForumException(paramName);
    }
}
