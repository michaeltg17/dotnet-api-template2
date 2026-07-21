namespace IntegrationTests.Settings
{
    public interface ITestSettings
    {
        bool KeepAliveDatabase { get; }
        bool EnableSqlLogging { get; }
    }
}
