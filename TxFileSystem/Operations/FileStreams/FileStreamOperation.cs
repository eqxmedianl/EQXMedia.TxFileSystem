namespace EQXMedia.TxFileSystem.Operations.FileStreams
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Operations.Files;
    using Microsoft.Win32.SafeHandles;
    using System;

    internal abstract class FileStreamOperation : FileStreamBackupOperation, IEnlistmentOperation
    {
        protected FileStreamOperation(ITxFileStream fileStream, string path)
            : base(fileStream, path)
        {
        }

        protected FileStreamOperation(ITxFileStream fileStream, IntPtr handle, string path)
            : base(fileStream, handle, path)
        {
        }

        protected FileStreamOperation(ITxFileStream fileStream, SafeFileHandle safeFileHandle)
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
            if (OperationRollbackGuard.ShouldRollback(this.OperationType, ((TxFileStream)_fileStream)._txFileSystem.Journal.State))
            {
                Restore();
            }
        }

        public void Journalize(IOperation operation)
        {
            ((TxFileStream)_fileStream)._txFileSystem.Journal.Add(operation);
        }
    }
}