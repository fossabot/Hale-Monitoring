namespace Hale.Lib.Utilities
{
    using System;

    [CLSCompliant(false)]
    public static class StorageUnitFormatter
    {
        public static string HumanizeStorageUnit(ulong p)
        {
            return HumanizeStorageUnit((float)p);
        }

        public static string HumanizeStorageUnit(long p)
        {
            return HumanizeStorageUnit((float)p);
        }

        public static string HumanizeStorageUnit(int p)
        {
            return HumanizeStorageUnit((float)p);
        }

        public static string HumanizeStorageUnit(double p)
        {
            return HumanizeStorageUnit((float)p);
        }

        public static string HumanizeStorageUnit(float p)
        {
            const ulong TER = 1099511627776;
            const ulong GIG = 1073741824;
            const ulong MEG = 1048576;
            const ulong KIL = 1024;

            if (p >= TER)
            {
                return (p / TER).ToString("F1") + "TB ";
            }
            else if (p >= GIG)
            {
                return (p / GIG).ToString("F1") + "GB ";
            }
            else if (p >= MEG)
            {
                return (p / MEG).ToString("F1") + "MB ";
            }
            else if (p >= KIL)
            {
                return (p / KIL).ToString("F1") + "KB ";
            }
            else
            {
                return p.ToString() + "B ";
            }
        }
    }
}
