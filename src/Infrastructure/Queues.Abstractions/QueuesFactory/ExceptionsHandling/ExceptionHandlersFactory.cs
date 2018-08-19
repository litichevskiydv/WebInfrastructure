namespace Skeleton.Queues.Abstractions.QueuesFactory.ExceptionsHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Handlers;
    using QueuesFactory;

    public class ExceptionHandlersFactory<TMessageDescription> : IExceptionHandlersFactory<TMessageDescription>
        where TMessageDescription : QueueMessageDescriptionBase, new()
    {
        private readonly IReadOnlyDictionary<ExceptionHandlingPolicy, ExceptionHandlerBase<TMessageDescription>> _supportedPolicies;

        public ExceptionHandlersFactory(IEnumerable<ExceptionHandlerBase<TMessageDescription>> registerdHandlers)
        {
            if(registerdHandlers == null)
                throw new ArgumentNullException(nameof(registerdHandlers));

            var registeredHandlersArray = registerdHandlers.ToArray();
            if (registeredHandlersArray.GroupBy(x => x.ExceptionHandlingPolicy).Any(x => x.Count() > 1))
                throw new InvalidOperationException("More than one handler was registered for policy");

            _supportedPolicies = registeredHandlersArray.ToDictionary(x => x.ExceptionHandlingPolicy);
        }

        public ExceptionHandlerBase<TMessageDescription> CreateHandler(ExceptionHandlingPolicy policy)
        {
            if(_supportedPolicies.TryGetValue(policy, out var handler) == false)
                throw new NotSupportedException($"Policy {policy.ToString()} is not supported");

            return handler;
        }
    }
}