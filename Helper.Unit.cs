namespace Helper;

internal static class Unit
{
    public static string Parse(int @base, string[] units, double num)
    {
        int unit = 0;
        while (@base > 1)
        {
            if (num.CompareTo(Math.Pow(@base, unit + 1)) is -1 || unit == units.Length - 1)
            {
                return $"{num / Math.Pow(@base, unit):f2} {units[unit]}";
            }
            unit++;
        }
        throw new NotImplementedException();
    }
}
