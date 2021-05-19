namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Collections.Generic;
    using System.IO;

    internal sealed class EnumerateFileSystemEntriesOperation : DirectoryOperation, IReturningOperation<IEnumerable<string>>
    {
        private readonly string _searchPattern = null;
        private readonly SearchOption? _searchOption;
#if ENUMERATING_IO
        private readonly EnumerationOptions _enumerationOptions = null;
#endif

        public EnumerateFileSystemEntriesOperation(TxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public EnumerateFileSystemEntriesOperation(TxDirectory directory, string path, string searchPattern)
            : this(directory, path)
        {
            _searchPattern = searchPattern;
        }

        public EnumerateFileSystemEntriesOperation(TxDirectory directory, string path, string searchPattern,
            SearchOption searchOption)
            : this(directory, path, searchPattern)
        {
            _searchOption = searchOption;
        }

#if ENUMERATING_IO
        public EnumerateFileSystemEntriesOperation(TxDirectory directory, string path, string searchPattern,
            EnumerationOptions enumerationOptions)
            : this(directory, path, searchPattern)
        {
            _enumerationOptions = enumerationOptions;
        }
#endif

        public override OperationType OperationType => OperationType.Info;

        public IEnumerable<string> Execute()
        {
            Journalize(this);

            if (_searchOption.HasValue)
            {
                return _directory.TxFileSystem.FileSystem.Directory.EnumerateFileSystemEntries(_path, _searchPattern, _searchOption.Value);
            }

#if ENUMERATING_IO
            if (_enumerationOptions != null)
            {
                return _directory.TxFileSystem.FileSystem.Directory.EnumerateFileSystemEntries(_path, _searchPattern, _enumerationOptions);
            }
#endif

            if (_searchPattern != null)
            {
                return _directory.TxFileSystem.FileSystem.Directory.EnumerateFileSystemEntries(_path, _searchPattern);
            }

            return _directory.TxFileSystem.FileSystem.Directory.EnumerateFileSystemEntries(_path);
        }
    }
}
