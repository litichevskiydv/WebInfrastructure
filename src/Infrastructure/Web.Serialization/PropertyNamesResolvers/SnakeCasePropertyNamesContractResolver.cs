namespace Skeleton.Web.Serialization.PropertyNamesResolvers
{
    using Newtonsoft.Json.Serialization;

    public class SnakeCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public SnakeCasePropertyNamesContractResolver() : base(true)
        {
            NamingStrategy = new SnakeCaseNamingStrategy(true, true);
        }
    }
}