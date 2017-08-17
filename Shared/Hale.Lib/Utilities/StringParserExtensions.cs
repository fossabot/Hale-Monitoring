namespace Hale.Lib.Utilities
{
    public static class StringParserExtensions
    {
        public static(string eaten, bool success, string rest) EatUntil(this string input, char end)
        {
            if (!string.IsNullOrEmpty(input))
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == end)
                    {
                        return (input.Substring(0, i), true, input.Substring(i));
                    }
                }
            }

            return (string.Empty, false, input);
        }

        public static(string eaten, bool success, string rest) EatChars(this string input, int count)
        {
            if (input.Length < count)
            {
                return (string.Empty, false, input);
            }

            return (input.Substring(0, count), true, input.Substring(count));
        }
    }
}
