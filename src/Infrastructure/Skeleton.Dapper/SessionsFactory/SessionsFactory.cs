namespace Skeleton.Dapper.SessionsFactory
{
    using System;
    using System.Data;
    using ConnectionsFactory;

    public class SessionsFactory : ISessionsFactory
    {
        private readonly IConnectionsFactory _connectionsFactory;

        public SessionsFactory(IConnectionsFactory connectionsFactory)
        {
            if (connectionsFactory == null)
                throw new ArgumentNullException(nameof(connectionsFactory));

            _connectionsFactory = connectionsFactory;
        }

        public ISession Create(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var connection = _connectionsFactory.Create();
            var transaction = connection.BeginTransaction(isolationLevel);

            return new Session(connection, transaction);
        }
    }
}