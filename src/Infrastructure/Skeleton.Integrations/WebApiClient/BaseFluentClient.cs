namespace Skeleton.Integrations.WebApiClient
{
    using System.Collections.Generic;

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