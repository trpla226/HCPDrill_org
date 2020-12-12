public static class IntExt
{
    public static bool IsValidHCP(this int hcp)
    {
        return hcp >= 0 && hcp <= 37;
    }
}
