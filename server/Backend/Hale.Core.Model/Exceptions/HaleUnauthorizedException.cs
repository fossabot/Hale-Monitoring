namespace Hale.Core.Model.Exceptions
{
    using System;

    [Serializable]
    public class HaleUnauthorizedException : Exception
    {
        public HaleUnauthorizedException()
            : base()
        {
        }

        public HaleUnauthorizedException(string text)
            : base(text)
        {
        }
    }
}
