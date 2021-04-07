namespace EQXMedia.TxFileSystem.Abstractions
{
    using global::EQXMedia.TxFileSystem.Operations;

    public interface IBackupOperation
    {
        public OperationType OperationType { get; }

        public string BackupPath { get; }

        public void Backup();

        public void Delete();

        public void Restore();
    }
}