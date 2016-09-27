using PowerUp.Mvc.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace PowerUp.Mvc
{
	internal class FieldInfo
	{
		public string Placeholder { get; private set; }
		public string FieldValue { get; private set; }
		public string FieldName { get; private set; }
		public string InputType { get; private set; }
		public string FontAwesomeIcon { get; private set; }
		public string CustomStyle { get; private set; }

		public FieldInfo(MemberInfo fieldInfo, object model)
		{
			var displayAttr = fieldInfo.GetCustomAttribute<DisplayAttribute>();
			var dataTypeAttr = fieldInfo.GetCustomAttribute<DataTypeAttribute>();
			var faAttr = fieldInfo.GetCustomAttribute<FontAwesomeAttribute>();

			FieldName = fieldInfo.Name;
			FieldValue = model == null ? "" : model.GetPropertyValue<string>(fieldInfo.Name);
			InputType = dataTypeAttr != null && dataTypeAttr.DataType == DataType.Password ? "password" : "text";
			Placeholder = displayAttr == null ? fieldInfo.Name.ToDisplayName() : displayAttr.Name;
			FontAwesomeIcon = faAttr != null ? string.Format("icon-append fa {0}", faAttr.CssClass) : "";
			CustomStyle = string.IsNullOrWhiteSpace(FontAwesomeIcon) ? "style='padding-left:0px'" : "";
		}
		 
		public FieldInfo(Expression fieldSelector, object model)
			: this(GetMemberInfoFromFieldSelector(fieldSelector), model) { }

		private static MemberInfo GetMemberInfoFromFieldSelector(Expression fieldSelector)
		{
			return ((fieldSelector as LambdaExpression).Body as MemberExpression).Member;
		}
	}
}
