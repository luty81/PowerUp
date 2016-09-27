using System;

namespace PowerUp.Mvc.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FontAwesomeAttribute: Attribute
    {
        public string CssClass { get; set; }

        public FontAwesomeAttribute(string cssClass = null)
        {
            CssClass = cssClass;
        }
    }
}
