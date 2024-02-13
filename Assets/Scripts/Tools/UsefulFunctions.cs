public static class UsefulFunctions
{
    public static bool IsBetween(float value, float a, float b)
    {
        if (a > b) return value >= b && value <= a;
        return value >= a && value <= b;
    }
}
