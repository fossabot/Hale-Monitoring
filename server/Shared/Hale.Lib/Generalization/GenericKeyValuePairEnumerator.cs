namespace Hale.Lib.Generalization
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    internal class GenericKeyValuePairEnumerator<T> : IEnumerator<KeyValuePair<string, T>>
        where T : class
    {
        private IEnumerator<KeyValuePair<string, object>> enu;

        public GenericKeyValuePairEnumerator(IEnumerator<KeyValuePair<string, object>> enu)
        {
            this.enu = enu;
        }

        public object Current
            => this.enu.Current;

        KeyValuePair<string, T> IEnumerator<KeyValuePair<string, T>>.Current
            => new KeyValuePair<string, T>(this.enu.Current.Key, this.enu.Current.Value as T);

        public void Dispose()
            => this.enu.Dispose();

        public bool MoveNext()
            => this.enu.MoveNext();

        public void Reset()
            => this.enu.Reset();
    }
}
