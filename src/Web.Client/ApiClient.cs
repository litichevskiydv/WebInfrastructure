namespace Web.Client
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.Options;
    using ServicesClients.AccountController;
    using ServicesClients.ValuesService;
    using Skeleton.Web.Integration.BaseApiFluentClient;

    public partial class ApiClient : BaseFluentClient
    {
        public ApiClient(HttpClient httpClient, IOptions<ApiClientOptions> clientOptions)
        {
            if(httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));
            if (clientOptions == null)
                throw new ArgumentNullException(nameof(clientOptions));

            var optionsValue = clientOptions.Value;

            _valuesServiceClient = new ValuesServiceClient(
                httpClient,
                Options.Create(
                    new ValuesServiceClientOptions
                    {
                        BaseUrl = optionsValue.BaseUrl,
                        Timeout = optionsValue.Timeout,
                        Serializer = optionsValue.Serializer
                    }
                )
            );
            ServicesClients.Add(_valuesServiceClient);

            _accountControllerClient = new AccountControllerClient(
                httpClient,
                Options.Create(
                    new AccountControllerClientOptions
                    {
                        BaseUrl = optionsValue.BaseUrl,
                        Timeout = optionsValue.Timeout,
                        Serializer = optionsValue.Serializer
                    }
                )
            );
            ServicesClients.Add(_accountControllerClient);
        }
    }
}