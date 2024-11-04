using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using WebApp.Core.Models;

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

            if (int.IsNegative(argument.UserId) ||
                int.IsNegative(argument.TopicId) ||
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

            if (string.IsNullOrEmpty(argument.Content) ||
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

            if (string.IsNullOrEmpty(argument.Content) ||
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

            if (string.IsNullOrEmpty(argument.Reason) ||
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
                int.IsNegative(argument.UserId) ||
                int.IsNegative(argument.MessageId) ||
                Enum.IsDefined(argument.Status))
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
                int.IsNegative(argument.UserId) ||
                int.IsNegative(argument.MessageId) ||
                Enum.IsDefined(argument.Status))
            {
                Throw(paramName);
            }
        }

        public static void ThrowIfTopicCreateModelIsNotCorrect([NotNull] TopicCreateModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (string.IsNullOrEmpty(argument.TitleId) ||
                string.IsNullOrEmpty(argument.Description) ||
                argument.CreatedAt < new DateTime(2024, 3, 10) ||
                int.IsNegative(argument.UserId))
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
                int.IsNegative(argument.UserId))
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
                argument.CreatedAt < new DateTime(2024, 3, 10))
            {
                Throw(paramName);
            }
        }


        public static void ThrowIfUserCreateModelIsNotCorrect([NotNull] UserCreateModel? argument,
            [CallerArgumentExpression(nameof(argument))]
            string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            if (string.IsNullOrEmpty(argument.Nickname) ||
                string.IsNullOrEmpty(argument.Email) ||
                Enum.IsDefined(argument.Role) ||
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
                Enum.IsDefined(argument.Role) ||
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
