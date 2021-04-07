namespace EQXMedia.TxFileSystem.Journaling
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class TxJournalEntry
    {
        public TxJournalEntry(IOperation operation)
        {
            this.Operation = operation;
        }

        public IOperation Operation { get; }
    }
}