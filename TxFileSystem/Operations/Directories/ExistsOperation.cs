namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class ExistsOperation : DirectoryOperation, IReturningOperation<bool>
    {
        public ExistsOperation(ITxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public override OperationType OperationType => OperationType.Info;

        public bool Execute()
        {
            Journalize(this);

            return _directory.TxFileSystem.FileSystem.Directory.Exists(_path);
        }
    }
}
