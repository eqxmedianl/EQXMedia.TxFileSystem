namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class GetFileSystemEntriesOperation : DirectoryOperation, IReturningOperation<string[]>
    {
        private readonly string _searchPattern = null;

        public GetFileSystemEntriesOperation(TxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public GetFileSystemEntriesOperation(TxDirectory directory, string path, string searchPattern)
            : this(directory, path)
        {
            _searchPattern = searchPattern;
        }

        public override OperationType OperationType => OperationType.Info;

        public string[] Execute()
        {
            Journalize(this);

            if (_searchPattern == null)
            {
                return _directory.TxFileSystem.FileSystem.Directory.GetFileSystemEntries(_path);
            }

            return _directory.TxFileSystem.FileSystem.Directory.GetFileSystemEntries(_path, _searchPattern);
        }
    }
}
