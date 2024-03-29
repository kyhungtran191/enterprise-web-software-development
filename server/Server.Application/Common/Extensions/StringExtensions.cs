using Server.Domain.Entity.Content;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Server.Application.Common.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveAccents(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                            != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string Slugify(this string phrase)
        {
          
            string output = phrase.RemoveAccents().ToLower();
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");
            output = Regex.Replace(output, @"\s+", " ").Trim();
            output = Regex.Replace(output, @"\s", "-");
            return output;
        }
        public static string ToStringValue(this ContributionStatus status)
        {
            switch (status)
            {
                case ContributionStatus.Pending:
                    return "PENDING";
                case ContributionStatus.Approve:
                    return "APPROVED";
                case ContributionStatus.Reject:
                    return "REJECTED";
                default:
                    return "UNKNOWN";
            }
        }
    }
}
