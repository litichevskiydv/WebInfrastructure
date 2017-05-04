namespace Skeleton.Web.Integration.BaseApiFluentClient
{
    using System.Collections.Generic;
    using BaseApiClient;

    public abstract class BaseFluentClient
    {
        protected readonly List<BaseClient> ServicesClients;
        public dynamic CurrentState { get; protected set; }

        protected BaseFluentClient()
        {
            ServicesClients = new List<BaseClient>();
        }
    }
}