public static class DateOnlyExtensions
{
    public static int ToUntisDate(this DateOnly date)
        => date.Year * 10000 + date.Month * 100 + date.Day;

    public static DateOnly FromUntisDate(int value)
        => new(value / 10000, value / 100 % 100, value % 100);
}
