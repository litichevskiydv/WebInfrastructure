namespace Skeleton.CQRS.Implementations.Commands.CommandsFactory
{
    using System;
    using Abstractions.Commands;

    /// <summary>
    /// Commands factory implementation based on IServiceProvider
    /// </summary>
    public class GenericCommandsFactory : ICommandsFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCommandsFactory(IServiceProvider serviceProvider)
        {
            if(serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public ICommand<TCommandContext> CreateCommand<TCommandContext>() where TCommandContext : ICommandContext
        {
            return (ICommand<TCommandContext>)_serviceProvider.GetService(typeof(ICommand<TCommandContext>));
        }

        /// <inheritdoc />
        public IAsyncCommand<TCommandContext> CreateAsyncCommand<TCommandContext>() where TCommandContext : ICommandContext
        {
            return (IAsyncCommand<TCommandContext>)_serviceProvider.GetService(typeof(IAsyncCommand<TCommandContext>));
        }
    }
}