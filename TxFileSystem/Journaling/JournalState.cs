namespace EQXMedia.TxFileSystem.Journaling
{
    internal enum JournalState
    {
        Initial,
        Committing,
        Committed,
        InDoubt,
        Preparing,
        Prepared,
        RollingBack,
        RolledBack
    }
}
