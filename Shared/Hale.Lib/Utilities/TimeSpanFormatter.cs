namespace Hale.Lib.Utilities
{
    using System;
    using System.Text;

    public static class TimeSpanFormatter
    {
        public static string HumanizeTimeSpan(TimeSpan ts)
        {
            var sb = new StringBuilder();

            if (ts.Days > 0)
            {
                sb.Append(ts.Days + "d ");
            }

            if (ts.Hours > 0)
            {
                sb.Append(ts.Hours + "h ");
            }

            if (ts.Minutes > 0)
            {
                sb.Append(ts.Minutes + "m ");
            }

            sb.Append(ts.Seconds + "s");

            return sb.ToString();
        }
    }
}
