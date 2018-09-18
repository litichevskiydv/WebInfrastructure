namespace Skeleton.Queues.RabbitMq.QueuesFactory.Configuration
{
    using System;
    using Abstractions.QueuesFactory;
    using Abstractions.QueuesFactory.ExceptionsHandling;
    using Abstractions.QueuesFactory.ExceptionsHandling.Handlers;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqSupport(this IServiceCollection serviceCollection)
        {
            if(serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddSingleton<ExceptionHandlerBase<RabbitMessageDescription>, EmptyExceptionHandler<RabbitMessageDescription>>();
            serviceCollection.AddSingleton<ExceptionHandlerBase<RabbitMessageDescription>, RequeuingExceptionHandler<RabbitMessageDescription>>();
            serviceCollection.AddSingleton<ExceptionHandlerBase<RabbitMessageDescription>, ErrorsQueuingExceptionHandler<RabbitMessageDescription>>();
            serviceCollection.TryAddSingleton<IExceptionHandlersFactory<RabbitMessageDescription>, ExceptionHandlersFactory<RabbitMessageDescription>>();
            serviceCollection.TryAddSingleton<RabbitQueuesFactory>();
            serviceCollection.TryAddSingleton<ITypedQueuesFactory<RabbitQueueCreationOptions>>(x => x.GetService<RabbitQueuesFactory>());
            serviceCollection.TryAddSingleton<IUntypedQueuesFactory<RabbitQueueCreationOptions, QueueBase<RabbitMessageDescription>>>(x => x.GetService<RabbitQueuesFactory>());
            serviceCollection.TryAddSingleton<IGenericQueuesFactory, GenericQueuesFactory>();

            return serviceCollection;
        }
    }
}