using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using PowerUp.Mvc.Annotations;

namespace PowerUp.Mvc
{
    public static class HtmlExtensions
    {
        public static IHtmlString TextFieldFor<TModel, TField>(this HtmlHelper<TModel> html, Expression<Func<TModel, TField>> fieldSelector)
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

            var sectionBuilder = new StringBuilder();
            sectionBuilder.AppendLine("<section>");
            sectionBuilder.AppendLine("\t<label class='input'>");
            sectionBuilder.AppendFormat("\t\t<i class='{0}'></i>", faIcon);
            sectionBuilder.AppendFormat("\t\t<input class='fix-placeholder' type='{2}' id='{0}' name='{0}' placeholder='{1}' value='{3}' />", field.Name, placeholder, inputType, fieldValue);
            sectionBuilder.AppendLine("\t</label>");
            sectionBuilder.AppendLine("</section>");
            return MvcHtmlString.Create(sectionBuilder.ToString());
        }

        public static IHtmlString FormBlockTextFieldFor<TModel, TField>(this HtmlHelper<TModel> html, Expression<Func<TModel, TField>> fieldSelector)
        {
            var fieldInfo = new FieldInfo(fieldSelector, html.ViewData.Model);
            var sectionBuilder = new StringBuilder();

            sectionBuilder.AppendLine("<div class='input-group margin-bottom-20'>");

            sectionBuilder.AppendLine("\t<span class='input-group-addon rounded-left'>");
            sectionBuilder.AppendFormat("\t\t<i class='{0}'></i>", fieldInfo.FontAwesomeIcon.Replace("icon-append", ""));
            sectionBuilder.AppendLine("\t</span>");
            sectionBuilder.AppendFormat(
                "\t\t<input type='{0}' class='form-control rounded-right fix-placeholder' name='{1}' placeholder='{2}' value='{3}' {4}/>",
                fieldInfo.InputType,
                fieldInfo.FieldName,
                fieldInfo.Placeholder,
                fieldInfo.FieldValue,
                fieldInfo.CustomStyle);

            sectionBuilder.AppendLine("</div>");

            return MvcHtmlString.Create(sectionBuilder.ToString());
        }
    }
}
