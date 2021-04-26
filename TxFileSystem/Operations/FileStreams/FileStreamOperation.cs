namespace EQXMedia.TxFileSystem.Operations.FileStreams
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Operations.Files;
    using Microsoft.Win32.SafeHandles;
    using System;

    internal abstract class FileStreamOperation : FileStreamBackupOperation, IEnlistmentOperation
    {
        protected FileStreamOperation(TxFileStream fileStream, string path)
            : base(fileStream, path)
        {
        }

        protected FileStreamOperation(TxFileStream fileStream, IntPtr handle)
            : base(fileStream, handle)
        {
        }

        protected FileStreamOperation(TxFileStream fileStream, SafeFileHandle safeFileHandle)
            : base(fileStream, safeFileHandle)
        {
        }

        public override abstract OperationType OperationType { get; }

        public void Commit()
        {
            Delete();
        }

        public virtual void Rollback()
        {
            if (OperationRollbackGuard.ShouldRollback(this.OperationType, _fileStream.TxFileSystem.Journal.State))
            {
                Restore();
            }
        }

        public void Journalize(IOperation operation)
        {
            _fileStream.TxFileSystem.Journal.Add(operation);
        }
    }
}