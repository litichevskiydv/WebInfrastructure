namespace Skeleton.Web.Serialization.JsonNet.PropertyNamesResolvers
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