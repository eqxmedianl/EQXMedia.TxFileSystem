namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal abstract class DirectoryOperation : DirectoryBackupOperation, IEnlistmentOperation
    {
        protected readonly ITxDirectory _directory;
        protected readonly string _path = null;

        protected DirectoryOperation(ITxDirectory directory, string path)
            : base(directory, path)
        {
            _directory = directory;
            _path = path;

            Backup();
        }

        protected DirectoryOperation(ITxDirectory directory)
            : this(directory, null)
        {
            _directory = directory;

            Backup();
        }

        public override abstract OperationType OperationType { get; }

        public void Commit()
        {
            Delete();
        }

        public void Journalize(IOperation operation)
        {
            ((TxDirectory)_directory)._txFileSystem.Journal.Add(operation);
        }

        public void Rollback()
        {
            if (OperationRollbackGuard.ShouldRollback(this.OperationType, ((TxDirectory)_directory)._txFileSystem.Journal.State))
            {
                Restore();
            }
        }
    }
}