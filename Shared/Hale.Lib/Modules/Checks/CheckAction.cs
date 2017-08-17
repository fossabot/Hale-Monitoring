namespace Hale.Lib.Modules.Checks
{
    using System;

    [Serializable]
    public class CheckAction
    {
        public string Action { get; set; }

        public string Module { get; set; }

        public string Target { get; set; }
    }
}
