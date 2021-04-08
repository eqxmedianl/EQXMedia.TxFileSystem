﻿namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class GetDirectoriesOperation : DirectoryOperation, IReturningOperation<string[]>
    {
        private readonly string _searchPattern = null;
        private readonly SearchOption? _searchOption;
#if !NETSTANDARD2_0
        private readonly EnumerationOptions _enumerationOptions = null;
#endif

        public GetDirectoriesOperation(ITxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public GetDirectoriesOperation(ITxDirectory directory, string path, string searchPattern)
            : this(directory, path)
        {
            _searchPattern = searchPattern;
        }

        public GetDirectoriesOperation(ITxDirectory directory, string path, string searchPattern,
            SearchOption searchOption)
            : this(directory, path, searchPattern)
        {
            _searchOption = searchOption;
        }

#if !NETSTANDARD2_0
        public GetDirectoriesOperation(ITxDirectory directory, string path, string searchPattern,
            EnumerationOptions enumerationOptions)
            : this(directory, path, searchPattern)
        {
            _enumerationOptions = enumerationOptions;
        }
#endif

        public override OperationType OperationType => OperationType.Info;

        public string[] Execute()
        {
            Journalize(this);

            if (_searchOption.HasValue)
            {
                return _directory.FileSystem.Directory.GetDirectories(_path, _searchPattern, _searchOption.Value);
            }

#if !NETSTANDARD2_0
            if (_enumerationOptions != null)
            {
                return _directory.FileSystem.Directory.GetDirectories(_path, _searchPattern, _enumerationOptions);
            }
#endif

            if (_searchPattern != null)
            {
                return _directory.FileSystem.Directory.GetDirectories(_path, _searchPattern);
            }

            return _directory.FileSystem.Directory.GetDirectories(_path);
        }
    }
}
