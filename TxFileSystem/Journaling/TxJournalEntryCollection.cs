namespace EQXMedia.TxFileSystem.Journaling
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal sealed class TxJournalEntryCollection : ICollection<TxJournalEntry>
    {
        private readonly ICollection<TxJournalEntry> _txJournalEntries;

        public TxJournalEntryCollection()
            : base()
        {
            _txJournalEntries = new Collection<TxJournalEntry>();
        }

        public int Count => _txJournalEntries.Count;

        public bool IsReadOnly => _txJournalEntries.IsReadOnly;

        public void Add(TxJournalEntry item)
        {
            if (!this.Contains(item))
            {
                _txJournalEntries.Add(item);
            }
        }

        public void Clear()
        {
            _txJournalEntries.Clear();
        }

        public bool Contains(TxJournalEntry item)
        {
            return _txJournalEntries.Contains(item);
        }

        public void CopyTo(TxJournalEntry[] array, int arrayIndex)
        {
            _txJournalEntries.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TxJournalEntry> GetEnumerator()
        {
            return _txJournalEntries.GetEnumerator();
        }

        public bool Remove(TxJournalEntry item)
        {
            throw new NotImplementedException("Journal entries are not meant to be removed from the collection");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _txJournalEntries.GetEnumerator();
        }
    }
}
