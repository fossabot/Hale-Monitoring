namespace Hale.Lib.Modules.Checks
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Serializable]
    public struct DataPoint
    {
        public DataPoint(string dataType, float value)
        {
            this.DataType = dataType;
            this.Value = value;
        }

        public string DataType { get; set; }

        public float Value { get; set; }

        public override string ToString()
        {
            return string.Concat(this.DataType, " => ", this.Value.ToString("F2"));
        }
    }

    [Serializable]
    public class Datapoints : List<DataPoint>
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < this.Count; i++)
            {
                sb.Append(this[i]);
                if (i < this.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
