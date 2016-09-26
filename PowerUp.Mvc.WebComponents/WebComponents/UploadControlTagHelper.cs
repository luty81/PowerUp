using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorEngine;
using RazorEngine.Templating;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IO;

namespace PowerUp.Mvc.WebComponents
{
    [HtmlTargetElement("upload")]
    public class UploadControlTagHelper: TagHelper
    {
        private const string UploadControlCaptionAttributeName = "caption";
        private const string UploadControlActionAttributeName = "action";

        [HtmlAttributeName(UploadControlCaptionAttributeName)]
        public string Caption { get; set; }

        [HtmlAttributeName(UploadControlActionAttributeName)]
        public string Action { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //var razor = 
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "UploadControl.cshtml");
            var template = File.ReadAllText(templatePath);
            var model = new { Title = Caption, Action = Action };
            var controlHtml = Engine.Razor.RunCompile(template, "UploadControl", null, model);

            //var uploadControlHtml =
            //    UploadControl
            //        .HTML
            //            .Replace("@title", Caption)
            //            .Replace("@action", Action);

            output.Content.AppendHtml(controlHtml);
        }
    }
}
