namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string Remove(this string thisString, string @string)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(thisString));
            return thisString.Replace(@string, string.Empty, StringComparison.Ordinal);
        }
    }
}
