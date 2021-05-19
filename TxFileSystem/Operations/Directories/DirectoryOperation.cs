namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal abstract class DirectoryOperation : DirectoryBackupOperation, IEnlistmentOperation
    {
        protected readonly string _path = null;

        protected DirectoryOperation(TxDirectory directory, string path)
            : base(directory, path)
        {
            _path = path;

            Backup();
        }

        protected DirectoryOperation(TxDirectory directory)
            : this(directory, null)
        {
            Backup();
        }

        public override abstract OperationType OperationType { get; }

        public void Commit()
        {
            Delete();
        }

        public void Journalize(IOperation operation)
        {
            _directory.TxFileSystem.Journal.Add(operation);
        }

        public void Rollback()
        {
            if (OperationRollbackGuard.ShouldRollback(this.OperationType, _directory.TxFileSystem.Journal.State))
            {
                Restore();
            }
        }
    }
}