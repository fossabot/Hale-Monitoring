namespace Hale.Lib.Generalization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    [Serializable]
    public class GenericValueCollection<T> : ICollection<T>
        where T : class
    {
        private ICollection<object> col;
        private IEnumerator<T> enu;

        public GenericValueCollection(ICollection<object> col)
        {
            if (col == null)
            {
                throw new ArgumentNullException(nameof(col));
            }

            this.col = col;
        }

        public int Count
            => this.col.Count;

        public bool IsReadOnly
            => this.col.IsReadOnly;

        private IEnumerator<T> Enu
            => this.enu != null
            ? this.enu
            : this.enu = new GenericValueEnumerator<T>(this.col.GetEnumerator());

        public void Add(T item)
            => this.col.Add(item);

        public void Clear()
            => this.col.Clear();

        public bool Contains(T item)
            => this.col.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
            => this.col.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator()
            => this.enu;

        public bool Remove(T item)
            => this.col.Remove(item);

        IEnumerator IEnumerable.GetEnumerator()
            => this.enu;
    }
}
