namespace Hale.Lib.Modules.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class UnknownUnsuccessfulException : Exception
    {
        private const string ExceptionMessage = "The function did not run successfully, but no exception was provided.";

        public UnknownUnsuccessfulException()
            : base(ExceptionMessage)
        {
        }

        public UnknownUnsuccessfulException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }
    }
}
