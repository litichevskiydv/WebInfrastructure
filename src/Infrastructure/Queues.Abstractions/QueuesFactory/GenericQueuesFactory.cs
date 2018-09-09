namespace Skeleton.Queues.Abstractions.QueuesFactory
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using Configuration;

    public class GenericQueuesFactory : IGenericQueuesFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Type _typedQueuesFactoryType;

        public GenericQueuesFactory(IServiceProvider serviceProvider)
        {
            if(serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _typedQueuesFactoryType = typeof(ITypedQueuesFactory<>);
        }

        public ITypedQueue<TMessage> Create<TMessage>(QueueCreationOptions creationOptions)
        {
            var creationOptionsType = creationOptions.GetType();
            var typedQueuesFactory = _serviceProvider.GetService(_typedQueuesFactoryType.MakeGenericType(creationOptionsType));
            var createMethodDefinition = typedQueuesFactory.GetType()
                .GetMethods()
                .Single(x => x.IsGenericMethod && x.Name == nameof(ITypedQueuesFactory<QueueCreationOptions>.Create))
                .MakeGenericMethod(typeof(TMessage));

            try
            {
                return (ITypedQueue<TMessage>) createMethodDefinition.Invoke(typedQueuesFactory, new object[] {creationOptions});
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }

            return default(ITypedQueue<TMessage>);
        }
    }
}