namespace Hale.Lib.Modules.Info
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Hale.Lib.Modules.Results;

    [Serializable]
    public class InfoResult : ModuleResult
    {
        public Dictionary<string, string> Items { get; set; } = new Dictionary<string, string>();

        public string ItemsAsString()
        {
            var sb = new StringBuilder();
            sb.Append("(");
            foreach (var kv in this.Items)
            {
                sb.Append($"{kv.Key}=>{kv.Value}, ");
            }

            if (this.Items.Count > 0)
            {
                sb.Remove(sb.Length - 2, 2);
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
