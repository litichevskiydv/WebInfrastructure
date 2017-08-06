namespace Skeleton.Web.Serialization.PropertyNamesResolvers
{
    using Newtonsoft.Json.Serialization;

    public class SnakeCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public SnakeCasePropertyNamesContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy(true, true);
        }
    }
}