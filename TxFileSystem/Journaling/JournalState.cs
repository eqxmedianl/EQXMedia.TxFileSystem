namespace EQXMedia.TxFileSystem.Journaling
{
    public enum JournalState
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
