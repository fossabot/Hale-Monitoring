namespace Hale.Lib.Modules.Results
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    public class ResultRecordChunk : Dictionary<Guid, object>
    {
        public ResultRecordChunk()
        {
        }

        protected ResultRecordChunk(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
