namespace Skeleton.CQRS.Abstractions.Commands
{
    /// <summary>
    /// Commands dispatcher interface
    /// </summary>
    public interface ICommandsDispatcher
    {
        /// <summary>
        /// Method for synchronous commands execution
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <param name="command">Information needed for commands execution</param>
        void Execute<TCommandContext>(TCommandContext command) where TCommandContext : ICommandContext;

        /// <summary>
        /// Method for asynchronous commands execution
        /// </summary>
        /// <typeparam name="TCommandContext">Command context type</typeparam>
        /// <param name="command">Information needed for commands execution</param>
        void ExecuteAsync<TCommandContext>(TCommandContext command) where TCommandContext : ICommandContext;
    }
}