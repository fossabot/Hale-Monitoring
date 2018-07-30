namespace Hale.Lib.JsonRpc
{
    using System;

    public class JsonRpcError
    {
        public JsonRpcErrorCode Code { get; set; }

        public string Message { get; set; }

        public Exception Data { get; set; }
    }
}
