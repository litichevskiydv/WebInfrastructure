namespace Skeleton.Web
{
    using System;

    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}