namespace EQXMedia.TxFileSystem.Abstractions
{
    using global::EQXMedia.TxFileSystem.Operations;

    public interface IBackupOperation
    {
        OperationType OperationType { get; }

        string BackupPath { get; }

        void Backup();

        void Delete();

        void Restore();
    }
}