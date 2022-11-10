namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;
    using System.IO;

    internal sealed class GetFileSystemEntriesOperation : DirectoryOperation, IReturningOperation<string[]>
    {
        private readonly string _searchPattern = null;
        private readonly Nullable<SearchOption> _searchOption = null;

        public GetFileSystemEntriesOperation(TxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public GetFileSystemEntriesOperation(TxDirectory directory, string path, string searchPattern)
            : this(directory, path)
        {
            _searchPattern = searchPattern;
        }

        public GetFileSystemEntriesOperation(TxDirectory directory, string path, string searchPattern, SearchOption searchOption)
            : this(directory, path, searchPattern)
        {
            _searchOption = searchOption;
        }

        public override OperationType OperationType => OperationType.Info;

        public string[] Execute()
        {
            Journalize(this);

            if (_searchPattern == null)
            {
                return _directory.TxFileSystem.FileSystem.Directory.GetFileSystemEntries(_path);
            }

            if (!_searchOption.HasValue)
            {
                return _directory.TxFileSystem.FileSystem.Directory.GetFileSystemEntries(_path, _searchPattern);
            }

            return _directory.TxFileSystem.FileSystem.Directory.GetFileSystemEntries(_path, _searchPattern, _searchOption.Value);
        }
    }
}
