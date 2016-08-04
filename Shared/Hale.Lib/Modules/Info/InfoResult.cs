using Hale.Lib.Generalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Info
{
    [Serializable]
    public class InfoResult: ModuleResult
    {
        public Dictionary<string, string> Items { get; set; } = new Dictionary<string, string>();
        //public InfoItems Items { get; set; } = new InfoItems();

        public string ItemsAsString()
        {
            var sb = new StringBuilder();
            sb.Append("(");
            foreach (var kv in Items)
            {
                sb.Append($"{kv.Key}=>{kv.Value}, ");
            }
            if(Items.Count>0)
                sb.Remove(sb.Length - 2, 2);
            sb.Append(")");
            return sb.ToString();
        }
    }


    [Serializable]
    public class InfoResultRecord : ModuleResultRecord
    {
        GenericValueDictionary<InfoResult> _infoResults;
        public GenericValueDictionary<InfoResult> InfoResults
        {
            get
            {
                if (_infoResults == null)
                    _infoResults = new GenericValueDictionary<InfoResult>(Results);
                return _infoResults;
            }
        }
    }
}
