namespace Skeleton.Web.Tests.Integration.BaseApiClient
{
    using System;
    using System.Net.Http;
    using JetBrains.Annotations;
    using Web.Integration.BaseApiClient;
    using Web.Integration.BaseApiClient.Configuration;
    using Xunit;

    public class ServiceCollectionExtensionsTests
    {
        private interface IFakeClient
        {
            int Get();
        }

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        private class FakeClient : BaseClient, IFakeClient
        {
            public FakeClient(HttpClient httpClient, BaseClientOptions options) : base(httpClient, options)
            {
            }

            public int Get()
            {
                return 5;
            }
        }

        [Fact]
        public void AddClientWithoutInterfaceShouldNotPermitNullServiceCollection()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddClient<FakeClient>(null));
        }

        [Fact]
        public void AddClientWithInterfaceShouldNotPermitNullServiceCollection()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddClient<IFakeClient, FakeClient>(null));
        }
    }
}