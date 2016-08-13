using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hale.Lib.Modules.Actions;

namespace Hale.Lib.Generalization
{

    [Serializable]
    public class GenericValueDictionary<T> : IDictionary<string, T> where T : class
    {
        Dictionary<string, object> _dir;

        GenericKeyValuePairEnumerator<T> _kvpEnumerator;
        GenericKeyValuePairEnumerator<T> KvpEnumerator
        {
            get {
                if (_kvpEnumerator == null) _kvpEnumerator = new GenericKeyValuePairEnumerator<T>(_dir.GetEnumerator());
                return _kvpEnumerator;
            }
        }

        GenericValueCollection<T> _valCollection;
        GenericValueCollection<T> ValCollection
        {
            get
            {
                if (_valCollection == null) _valCollection = new GenericValueCollection<T>(_dir.Values);
                return _valCollection;
            }
        }

        public GenericValueDictionary(Dictionary<string, object> dir)
        {
            _dir = dir;
        }

        public T this[string key] {
            get { return (T)_dir[key]; }
            set { _dir[key] = value; }
        }

        public int Count { get { return _dir.Count; } }

        public bool IsReadOnly { get { return false; } }

        public ICollection<string> Keys { get { return _dir.Keys; } }

        public ICollection<T> Values { get { return ValCollection; } }

        public void Add(KeyValuePair<string, T> item) { _dir.Add(item.Key, item.Value); }

        public void Add(string key, ActionResult value) { _dir.Add(key, value); }

        public void Clear() { _dir.Clear(); }

        public bool Contains(KeyValuePair<string, T> item) { return _dir.Contains(new KeyValuePair<string, object>(item.Key, item.Value)); }

        public bool ContainsKey(string key) { return _dir.ContainsKey(key); }
        public void Add(string key, T value)
        {
            _dir.Add(key, value);
        }

        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) { return; }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() { return KvpEnumerator; }

        public bool Remove(KeyValuePair<string, T> item) { return _dir.Remove(item.Key); }

        public bool Remove(string key) { return _dir.Remove(key); }

        public bool TryGetValue(string key, out T value) {
            object o;
            var r = _dir.TryGetValue(key, out o);
            value = r ? (T)o : null;
            return r;
        }

        IEnumerator IEnumerable.GetEnumerator() { return KvpEnumerator; }
    }

    [Serializable]
    class GenericValueCollection<T> : ICollection<T> where T : class
    {
        ICollection<object> _col;

        IEnumerator<T> _enu;
        IEnumerator<T> enu {
                get {
                    if (_enu == null) _enu = new GenericValueEnumerator<T>(_col.GetEnumerator());
                    return _enu;
                }
        }

        public GenericValueCollection(ICollection<object> col) { _col = col; }

        public int Count {
            get { return _col.Count; }
        }

        public bool IsReadOnly {
            get { return _col.IsReadOnly; }
        }

        public void Add(T item) { _col.Add(item); }

        public void Clear() { _col.Clear(); }

        public bool Contains(T item) { return _col.Contains(item); }

        public void CopyTo(T[] array, int arrayIndex) { _col.CopyTo(array, arrayIndex); }

        public IEnumerator<T> GetEnumerator() { return enu; }

        public bool Remove(T item) { return _col.Remove(item); }

        IEnumerator IEnumerable.GetEnumerator() { return enu; }

    }

    [Serializable]
    class GenericValueEnumerator<T> : IEnumerator<T> where T : class
    {
        IEnumerator<object> _enu;
        public GenericValueEnumerator(IEnumerator<object> enu) { _enu = enu; }

        public T Current {
            get { return _enu.Current as T; }
        }

        object IEnumerator.Current {
            get { return Current; }
        }

        public void Dispose() { _enu.Dispose(); }
        public bool MoveNext() { return _enu.MoveNext(); }
        public void Reset() { _enu.Reset(); }
    }

    [Serializable]
    class GenericKeyValuePairEnumerator<T> : IEnumerator<KeyValuePair<string, T>> where T : class
    {
        IEnumerator<KeyValuePair<string, object>> _enu;

        public GenericKeyValuePairEnumerator(IEnumerator<KeyValuePair<string, object>> enu) { _enu = enu; }

        public object Current {
            get { return _enu.Current; }
        }

        KeyValuePair<string, T> IEnumerator<KeyValuePair<string, T>>.Current {
            get { return new KeyValuePair<string, T>(_enu.Current.Key, _enu.Current.Value as T); }
        }

        public void Dispose() { _enu.Dispose(); }
        public bool MoveNext() { return _enu.MoveNext(); }
        public void Reset() { _enu.Reset(); }
    }
}
