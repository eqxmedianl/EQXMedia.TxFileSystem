namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Collections.Generic;
    using System.IO;

    internal sealed class EnumerateFilesOperation : DirectoryOperation, IReturningOperation<IEnumerable<string>>
    {
        private readonly string _searchPattern = null;
        private readonly SearchOption? _searchOption;
#if !NETSTANDARD2_0 && !NET461
        private readonly EnumerationOptions _enumerationOptions = null;
#endif

        public EnumerateFilesOperation(ITxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public EnumerateFilesOperation(ITxDirectory directory, string path, string searchPattern)
            : this(directory, path)
        {
            _searchPattern = searchPattern;
        }

        public EnumerateFilesOperation(ITxDirectory directory, string path, string searchPattern,
            SearchOption searchOption)
            : this(directory, path, searchPattern)
        {
            _searchOption = searchOption;
        }

#if !NETSTANDARD2_0 && !NET461
        public EnumerateFilesOperation(ITxDirectory directory, string path, string searchPattern,
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
                return _directory.FileSystem.Directory.EnumerateFiles(_path, _searchPattern, _searchOption.Value);
            }

#if !NETSTANDARD2_0 && !NET461
            if (_enumerationOptions != null)
            {
                return _directory.FileSystem.Directory.EnumerateFiles(_path, _searchPattern, _enumerationOptions);
            }
#endif

            if (_searchPattern != null)
            {
                return _directory.FileSystem.Directory.EnumerateFiles(_path, _searchPattern);
            }

            return _directory.FileSystem.Directory.EnumerateFiles(_path);
        }
    }
}
