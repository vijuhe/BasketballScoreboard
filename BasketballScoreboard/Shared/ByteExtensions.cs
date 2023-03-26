namespace BasketballScoreboard.Shared
{
    public static class ByteExtensions
    {
        public static string ToPaddedString(this byte number)
        {
            return number.ToString().PadLeft(2, '0');
        }
    }
}
