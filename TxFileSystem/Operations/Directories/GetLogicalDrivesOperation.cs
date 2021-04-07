namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class GetLogicalDrivesOperation : DirectoryOperation, IReturningOperation<string[]>
    {
        public GetLogicalDrivesOperation(ITxDirectory directory)
            : base(directory)
        {
        }

        public override OperationType OperationType => OperationType.Info;

        public string[] Execute()
        {
            Journalize(this);

            return _directory.TxFileSystem.FileSystem.Directory.GetLogicalDrives();
        }
    }
}
