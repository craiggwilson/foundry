using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

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
    }
}