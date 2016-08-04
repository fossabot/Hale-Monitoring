namespace Hale.Lib.JsonRpc
{
    public enum JsonRpcErrorCode
    {
        Unknown = -1,

        // Parse errors
        ParseNotWellFormed = -32700,
        ParseUnsupportedEncoding = -32701,
        ParseInvalidCharacterForEncoding = -32702,

        // Server errors
        ServerInvalidJsonRpcNotConformingToSpec = -32600,
        ServerRequestedMethodNotFound = -32601,
        ServerInvalidMethodParameters = -32602,
        ServerInternalJsonRpcError = -32603,

        // Other
        ApplicationError = -32500,
        SystemError = -32400,
        TransportError = -32300,
    }
}