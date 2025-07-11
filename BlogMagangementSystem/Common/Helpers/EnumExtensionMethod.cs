using System.ComponentModel;

namespace BlogMagangementSystem.Common.Helpers
{
    public static class EnumExtensionMethod
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field is null)
                return value.ToString();

            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }

}
