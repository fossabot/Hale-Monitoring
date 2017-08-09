using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules
{
    public enum UnitType
    {
        Custom,
        Percent,
        StorageUnit,
        Time
    }

    public static class UnitTypeResolution 
    {
        public static class StorageUnit {
            public static string Byte = "byte";
            public static string Kilobyte = "kilobyte";
            public static string Megabyte = "megabyte";
            public static string Gigabyte = "gigabyte";
            public static string Kibibyte = "kibibyte";
            public static string Mebibyte = "mebibyte";
            public static string Gibibyte = "gibibyte";
        }

        public static class Time
        {
            public static string Millisecond = "millisecond";
            public static string Second = "second";
            public static string Minute = "minute";
            public static string Hour = "hour";
            public static string Day = "day";
            public static string Week = "week";
            public static string Month = "month";
            public static string Year = "year";
        }
    }

}
