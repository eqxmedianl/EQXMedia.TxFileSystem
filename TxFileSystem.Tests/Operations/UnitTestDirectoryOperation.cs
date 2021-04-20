namespace EQXMedia.TxFileSystem.Tests.Operations
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Operations;
    using global::EQXMedia.TxFileSystem.Operations.Directories;
    using System.Diagnostics;

    internal sealed class UnitTestDirectoryOperation : DirectoryBackupOperation, IEnlistmentOperation, IOperation
    {
        private readonly TxDirectory _txDirectory;

        public UnitTestDirectoryOperation(TxDirectory txDirectory, string path)
            : base(txDirectory, path)
        {
            _txDirectory = txDirectory;
        }

        public override OperationType OperationType => OperationType.Delete;

        public void Commit()
        {
            Debug.WriteLine("Committed operation");
        }

        public void Journalize(IOperation operation)
        {
            _txDirectory.TxFileSystem.Journal.Add(operation);
        }

        public void Rollback()
        {
            Debug.WriteLine("Rolled back operation");
        }
    }
}
