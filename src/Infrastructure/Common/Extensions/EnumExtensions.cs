namespace Skeleton.Common.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum member)
        {
            var memberTypeInfo = member.GetType().GetTypeInfo();

            if (memberTypeInfo.IsEnum == false)
                throw new ArgumentOutOfRangeException(nameof(member), "member is not enum");

            var fieldInfo = memberTypeInfo.GetField(member.ToString());
            if (fieldInfo == null)
                return null;

            var attributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>(false).AsArray();

            if (attributes.Length > 0)
                return attributes[0].Description;

            return member.ToString();
        }
    }
}