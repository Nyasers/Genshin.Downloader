namespace Helper;

internal static class StringH
{
    public static string? EmptyCheck(string? left)
    {
        return !string.IsNullOrEmpty(left) ? left : null;
    }

    public static string? WhiteSpaceCheck(string? left)
    {
        return !string.IsNullOrWhiteSpace(left) ? left : null;
    }

    public static string? GetKeyName(string key)
    {
        return key[(key.IndexOf('[') + 1)..key.IndexOf(']')];
    }
}
