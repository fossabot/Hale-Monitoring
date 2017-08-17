namespace Hale.Lib.Modules.Checks
{
    using System;

    [Serializable]
    public class CheckActions
    {
        public CheckAction Warning { get; set; }

        public CheckAction Critical { get; set; }
    }
}
