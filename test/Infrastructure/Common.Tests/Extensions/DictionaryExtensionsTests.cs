namespace Skeleton.Common.Tests.Extensions
{
    using System.Collections.Generic;
    using Common.Extensions;
    using Xunit;

    public class DictionaryExtensionsTests
    {
        [Fact]
        public void GetOrDefaultShouldReturnExistedValue()
        {
            // Given
            const int key = 1;
            const int expectedValue = 1;
            var dictionary = new Dictionary<int, int> {{key, expectedValue}};

            // When
            var actualValue = dictionary.GetOrDefault(key);

            // Then
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetOrDefaultShouldReturnDefaultValueForTypeIfKeyNotExist()
        {
            // Given
            const int key = 1;
            var dictionary = new Dictionary<int, object>();

            // When
            var actualValue = dictionary.GetOrDefault(key);

            // Then
            object expectedValue = null;
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetOrEmptyShouldReturnExistedValue()
        {
            // Given
            const int key = 1;
            const int expectedValue = 1;
            var dictionary = new Dictionary<int, int> {{key, expectedValue}};

            // When
            var actualValue = dictionary.GetOrEmpty(key);

            // Then
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetOrEmptyShouldReturnDefaultConstructorCallResultIfKeyNotExist()
        {
            // Given
            const int key = 1;
            var dictionary = new Dictionary<int, List<int>>();

            // When
            var actualValue = dictionary.GetOrEmpty(key);

            // Then
            Assert.Empty(actualValue);
        }

        [Fact]
        public void GetOrValueShouldReturnExistedValue()
        {
            // Given
            const int key = 1;
            const int expectedValue = 1;
            var dictionary = new Dictionary<int, int> {{key, expectedValue}};

            // When
            var actualValue = dictionary.GetOrValue(key, 5);

            // Then
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetOrValueShouldReturnPredefinedValueIfKeyNotExist()
        {
            // Given
            const int key = 1;
            const int expectedValue = 5;
            var dictionary = new Dictionary<int, int>();

            // When
            var actualValue = dictionary.GetOrValue(key, expectedValue);

            // Then
            Assert.Equal(expectedValue, actualValue); 
        }

        [Fact]
        public void GetOrValueWithFuncShouldReturnExistedValue()
        {
            // Given
            const int key = 1;
            const int expectedValue = 1;
            var dictionary = new Dictionary<int, int> {{key, expectedValue}};

            // When
            var actualValue = dictionary.GetOrValue(key, x => 5);

            // Then
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetOrValueWithFuncShouldReturnPredefinedValueIfKeyNotExist()
        {
            // Given
            const int key = 1;
            int ValueFunc(int x) => x + 1;
            var dictionary = new Dictionary<int, int>();

            // When
            var actualValue = dictionary.GetOrValue(key, ValueFunc);

            // Then
            var expectedValue = ValueFunc(key);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}