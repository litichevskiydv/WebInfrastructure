namespace Skeleton.CQRS.Implementations.Tests
{
    using System;
    using Abstractions.Queries;
    using JetBrains.Annotations;
    using Moq;
    using Queries;
    using Queries.QueriesFactory;
    using Xunit;

    public class QueriesDispatcherTests
    {
        [UsedImplicitly]
        public class NameQueryCtiterion : ICriterion
        {
        }

        private readonly Mock<IQueriesFactory> _mockQueriesFactory;
        private readonly QueriesDispatcher _queriesDispatcher;

        public QueriesDispatcherTests()
        {
            _mockQueriesFactory = new Mock<IQueriesFactory>();
            _queriesDispatcher = new QueriesDispatcher(_mockQueriesFactory.Object);
        }

        [Fact]
        public void ShouldExecuteQuery()
        {
            // Given
            const string expected = "abc";

            var queryMock = new Mock<IQuery<NameQueryCtiterion, string>>();
            queryMock.Setup(x => x.Ask(It.IsAny<NameQueryCtiterion>())).Returns(expected);
            _mockQueriesFactory.Setup(x => x.Create<NameQueryCtiterion, string>()).Returns(queryMock.Object);

            // When
            var actual = _queriesDispatcher.Execute<string>(new NameQueryCtiterion());

            // Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void QueryShouldThrowExceptionDuringTheExecutions()
        {
            // Given
            var queryMock = new Mock<IQuery<NameQueryCtiterion, string>>();
            queryMock.Setup(x => x.Ask(It.IsAny<NameQueryCtiterion>())).Throws<InvalidOperationException>();
            _mockQueriesFactory.Setup(x => x.Create<NameQueryCtiterion, string>()).Returns(queryMock.Object);

            // When, Then
            Assert.Throws<InvalidOperationException>(() => _queriesDispatcher.Execute<string>(new NameQueryCtiterion()));
        }
    }
}