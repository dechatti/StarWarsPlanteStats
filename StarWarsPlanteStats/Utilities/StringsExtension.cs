public static class StringsExtension
{
    public static int? ToIntOrNull(this string? input)
    {
        return int.TryParse(input, out int resultparsed) ? resultparsed : null;
    }
    public static long? ToLongOrNull(this string? input)
    {
        return long.TryParse(input, out long resultparsed) ? resultparsed : null;
    }
}

