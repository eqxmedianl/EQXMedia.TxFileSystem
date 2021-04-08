namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal abstract class FileOperation : FileBackupOperation, IEnlistmentOperation
    {
        protected readonly ITxFile _file = null;
        protected readonly string _path = null;

        protected FileOperation(ITxFile file, string path)
            : base(file, path)
        {
            _file = file;
            _path = path;

            Backup();
        }

        public override abstract OperationType OperationType { get; }

        public void Commit()
        {
            Delete();
        }

        public virtual void Rollback()
        {
            if (OperationRollbackGuard.ShouldRollback(this.OperationType, ((TxFile)_file)._txFileSystem.Journal.State))
            {
                Restore();
            }
        }

        public void Journalize(IOperation operation)
        {
            ((TxFile)_file)._txFileSystem.Journal.Add(operation);
        }
    }
}