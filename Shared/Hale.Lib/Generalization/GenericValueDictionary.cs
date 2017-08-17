namespace Hale.Lib.Generalization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Hale.Lib.Modules.Actions;

    [Serializable]
    public class GenericValueDictionary<T> : IDictionary<string, T>
        where T : class
    {
        private Dictionary<string, object> dir;
        private GenericKeyValuePairEnumerator<T> kvpEnumerator;
        private GenericValueCollection<T> valCollection;

        public GenericValueDictionary(Dictionary<string, object> dir)
        {
            this.dir = dir;
        }

        public int Count
            => this.dir.Count;

        public bool IsReadOnly
            => false;

        public ICollection<string> Keys
            => this.dir.Keys;

        public ICollection<T> Values
            => this.ValCollection;

        private GenericKeyValuePairEnumerator<T> KvpEnumerator
            => this.kvpEnumerator != null
            ? this.kvpEnumerator
            : this.kvpEnumerator = new GenericKeyValuePairEnumerator<T>(this.dir.GetEnumerator());

        private GenericValueCollection<T> ValCollection
            => this.valCollection != null
            ? this.valCollection
            : this.valCollection = new GenericValueCollection<T>(this.dir.Values);

        public T this[string key]
        {
            get { return (T)this.dir[key]; }
            set { this.dir[key] = value; }
        }

        public void Add(KeyValuePair<string, T> item)
            => this.dir.Add(item.Key, item.Value);

        public void Add(string key, ActionResult value)
            => this.dir.Add(key, value);

        public void Clear()
            => this.dir.Clear();

        public bool Contains(KeyValuePair<string, T> item)
            => this.dir.Contains(new KeyValuePair<string, object>(item.Key, item.Value));

        public bool ContainsKey(string key)
            => this.dir.ContainsKey(key);

        public void Add(string key, T value)
            => this.dir.Add(key, value);

        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            return;
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
            => this.KvpEnumerator;

        public bool Remove(KeyValuePair<string, T> item)
            => this.dir.Remove(item.Key);

        public bool Remove(string key)
            => this.dir.Remove(key);

        public bool TryGetValue(string key, out T value)
        {
            object o;
            var r = this.dir.TryGetValue(key, out o);
            value = r ? (T)o : null;
            return r;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.KvpEnumerator;
    }
}
