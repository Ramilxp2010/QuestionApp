using Ganss.Xss;
using Microsoft.AspNetCore.Components;

namespace QuestionAppUI.Helpers
{
    public static class SafeHtml
    {
        private static readonly HtmlSanitizer Sanitizer = CreateSanitizer();

        public static MarkupString ToMarkup(string html)
        {
            return new MarkupString(Sanitizer.Sanitize(html ?? string.Empty));
        }

        private static HtmlSanitizer CreateSanitizer()
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedAttributes.Add("target");
            return sanitizer;
        }
    }
}
