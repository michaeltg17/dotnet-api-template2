using Core.Extensions;
using Xunit;
using AwesomeAssertions;

namespace UnitTests.Core
{
    public class TypeExtensionsTests
    {
        [InlineData(typeof(int), "Int32")]
        [InlineData(typeof(List<>), "List")]
        [InlineData(typeof(Exception), "Exception")]
        [Theory]
        public void GetNameWithoutGenericArityTests(Type type, string expectedName)
        {
            //When
            var name = type.GetNameWithoutGenericArity();

            //Then
            name.Should().Be(expectedName);
        }
    }
}
