using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using PowerUp.Mvc.Annotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Html;
using System.Web;

namespace PowerUp.Mvc
{
    public static class HtmlExtensions
    {
        public static HtmlString TextFieldFor<TModel, TField>(this HtmlHelper<TModel> html, Expression<Func<TModel, TField>> fieldSelector)
        {
            var field = ((fieldSelector as LambdaExpression).Body as MemberExpression).Member;

            var displayAttr = field.GetCustomAttribute<DisplayAttribute>();
            var placeholder = displayAttr == null ? field.Name.ToDisplayName() : displayAttr.Name;

            var dataTypeAttr = field.GetCustomAttribute<DataTypeAttribute>();
            var inputType = dataTypeAttr != null && dataTypeAttr.DataType == DataType.Password ? "password" : "text";

            var faAttr = field.GetCustomAttribute<FontAwesomeAttribute>();
            var faIcon = faAttr != null ? string.Format("icon-append fa {0}", faAttr.CssClass) : "";

            var model = html.ViewData.Model;
            var fieldValue = model == null ? "" : model.GetPropertyValue<string>(field.Name);

            var htmlBuilder = new HtmlContentBuilder()
                .AppendLine("<section>")
                .AppendLine("<label class='input'>")
                .AppendLine($"<i class='{faIcon}'></i>")
                .AppendLine($"<input class='fix-placeholder' type='{inputType}' id='{field.Name}' name='{field.Name}' placeholder='{placeholder}' value='{fieldValue}' />")
                .AppendLine("</label>")
                .AppendLine("</section>");
            
            return new HtmlString(htmlBuilder.ToString());
        }

        public static HtmlString FormBlockTextFieldFor<TModel, TField>(this HtmlHelper<TModel> html, Expression<Func<TModel, TField>> fieldSelector)
        {
            var f = new FieldInfo(fieldSelector, html.ViewData.Model);

            var htmlBuilder = new HtmlContentBuilder()
                .Append("$<div class='input-group margin-bottom-20'>").AppendLine("\t")
                    .Append("$<span class='input-group-addon rounded-left'>").AppendLine("\t\t")
                    .Append($"<i class='{f.FontAwesomeIcon}'></i>".Replace("icon-append", "")).AppendLine("\t")
                .Append($"</span>").AppendLine("\t")
                .Append($"<input type='{f.InputType}' ")
                    .Append($"class='form-control rounded-right fix-placeholder'")
                    .Append($"name='{f.FieldName}' placeholder='{f.Placeholder}' value='{f.FieldValue}' {f.CustomStyle}/>")
                .AppendLine("</div>");

            return new HtmlString(htmlBuilder.ToString());
        }
    }
}
