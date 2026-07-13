using System.Reflection;
using Xunit.DependencyInjection;

namespace IntegrationTests
{
    internal class BeforeAfterTestConfiguration(
        WebApplicationFactoryFixture webApplicationFactoryFixture,
        ITestOutputHelperAccessor testOutputHelperAccessor)
        : BeforeAfterTest
    {
        public override ValueTask BeforeAsync(object? testClassInstance, MethodInfo methodUnderTest)
        {
            if (testClassInstance is Test test)
            {
                test.WebApplicationFactoryFixture = webApplicationFactoryFixture;
                test.TestOutputHelper = testOutputHelperAccessor.Output!;
                return test.Initialize();
            }

            return ValueTask.CompletedTask;
        }
    }
}
