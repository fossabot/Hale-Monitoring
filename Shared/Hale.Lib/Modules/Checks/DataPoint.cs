using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Lib.Modules.Checks
{
    [Serializable]
    public struct DataPoint
    {
        public string DataType { get; set; }
        public float Value { get; set; }
        public DataPoint(string DataType, float Value)
        {
            this.DataType = DataType;
            this.Value = Value;
        }

        public override string ToString()
        {
            return string.Concat(DataType, "=>", Value);
        }
    }

    [Serializable]
    public class Datapoints : List<DataPoint>
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < Count; i++)
            {
                sb.Append(this[i]);
                if (i < Count - 1)
                    sb.Append(", ");
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
