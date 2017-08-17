namespace Hale.Lib.Modules.Checks
{
    using System;

    [Serializable]
    public class CheckThresholds
    {
        public float Warning { get; set; }

        public float Critical { get; set; }
    }
}
