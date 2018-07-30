namespace Hale.Lib.Generalization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    [Serializable]
    internal class GenericValueEnumerator<T> : IEnumerator<T>
        where T : class
    {
        private IEnumerator<object> enu;

        public GenericValueEnumerator(IEnumerator<object> enu)
            => this.enu = enu;

        public T Current
            => this.enu.Current as T;

        object IEnumerator.Current
            => this.Current;

        public void Dispose()
            => this.enu.Dispose();

        public bool MoveNext()
            => this.enu.MoveNext();

        public void Reset()
            => this.enu.Reset();
    }
}
