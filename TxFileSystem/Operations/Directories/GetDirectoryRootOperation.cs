namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class GetDirectoryRootOperation : DirectoryOperation, IReturningOperation<string>
    {
        public GetDirectoryRootOperation(TxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public override OperationType OperationType => OperationType.Info;

        public string Execute()
        {
            Journalize(this);

            return _directory.TxFileSystem.FileSystem.Directory.GetDirectoryRoot(_path);
        }
    }
}
