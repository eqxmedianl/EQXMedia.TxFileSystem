namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Collections.Generic;
    using System.IO;

    internal sealed class EnumerateDirectoriesOperation : DirectoryOperation, IReturningOperation<IEnumerable<string>>
    {
        private readonly string _searchPattern = null;
        private readonly SearchOption? _searchOption;
        private readonly EnumerationOptions _enumerationOptions = null;

        public EnumerateDirectoriesOperation(ITxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public EnumerateDirectoriesOperation(ITxDirectory directory, string path, string searchPattern)
            : this(directory, path)
        {
            _searchPattern = searchPattern;
        }

        public EnumerateDirectoriesOperation(ITxDirectory directory, string path, string searchPattern,
            SearchOption searchOption)
            : this(directory, path, searchPattern)
        {
            _searchOption = searchOption;
        }

        public EnumerateDirectoriesOperation(ITxDirectory directory, string path, string searchPattern,
            EnumerationOptions enumerationOptions)
            : this(directory, path, searchPattern)
        {
            _enumerationOptions = enumerationOptions;
        }

        public override OperationType OperationType => OperationType.Info;

        public IEnumerable<string> Execute()
        {
            Journalize(this);

            if (_searchOption.HasValue)
            {
                return _directory.FileSystem.Directory.EnumerateDirectories(_path, _searchPattern, _searchOption.Value);
            }

            if (_enumerationOptions != null)
            {
                return _directory.FileSystem.Directory.EnumerateDirectories(_path, _searchPattern, _enumerationOptions);
            }

            if (_searchPattern != null)
            {
                return _directory.FileSystem.Directory.EnumerateDirectories(_path, _searchPattern);
            }

            return _directory.FileSystem.Directory.EnumerateDirectories(_path);
        }
    }
}
