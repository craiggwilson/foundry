using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;

namespace Foundry.Website.Extensions
{
    public static class HtmlExtensions
    {
        private static readonly Regex matcher = new Regex(@"(?<Link>\[\[(?<SubjectType>\w+):\s*?(?<Subject>.*?)]])", RegexOptions.Compiled);

        public static string ParseMessage(this HtmlHelper helper, string message)
        {
            return matcher.Replace(message, m =>
            {
                var subjectType = m.Groups["SubjectType"].Value;
                var subject = m.Groups["Subject"].Value;

                return helper.ActionLink(subject, "Index", subjectType, new { subject = subject }).ToHtmlString();
            });
        }

        public static string ToEnglish(this HtmlHelper helper, DateTime dateTime)
        {
            Func<int, string, string> toEnglish = (v, u) =>
            {
                if (v > 1)
                    u += "s";
                return string.Format("{0} {1}", v, u);
            };

            var now = DateTime.Now;
            int value;

            value = now.Year - dateTime.Year;
            if (now.Month < dateTime.Month)
                value--;

            if (value > 0)
                return toEnglish(value, "year");

            value = now.Month - dateTime.Month;
            if (value < 0)
                value += 12;

            if(value > 0)
                return toEnglish(value, "month");

            var timespan = now.Subtract(dateTime);
            value = timespan.Days;
            if(value > 0)
                return toEnglish(value, "day");

            value = timespan.Hours;
            if (value > 0)
                return toEnglish(value, "hour");

            value = timespan.Minutes;
            if (value > 0)
                return toEnglish(value, "minute");

            return toEnglish(timespan.Seconds, "second");
        }

        public static string SplitSourcePath(this HtmlHelper helper, Domain.Project project, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "";

            var parts = path.Split('/');
            
            var currentPath = "";
            var sb = new StringBuilder();
            for (int i = 0; i < parts.Length - 1; i++)
            {
                if (i > 0)
                {
                    currentPath += "/";
                    sb.Append("/");
                }

                currentPath += parts[i];
                sb.Append(helper.ActionLink(parts[i], "Source", "Project", new { account = project.AccountName, repository = project.RepositoryName, path = currentPath }, null));
            }

            if(parts.Length > 1)
                sb.Append("/");
            
            sb.Append(parts[parts.Length - 1]);

            return sb.ToString();
        }
    }
}