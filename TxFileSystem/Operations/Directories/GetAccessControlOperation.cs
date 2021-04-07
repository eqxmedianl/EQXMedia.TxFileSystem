﻿namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Security.AccessControl;

    internal sealed class GetAccessControlOperation : DirectoryOperation, IReturningOperation<DirectorySecurity>
    {
        private readonly AccessControlSections? _includeSections;

        public GetAccessControlOperation(ITxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public GetAccessControlOperation(ITxDirectory directory, string path,
            AccessControlSections includeSections)
            : this(directory, path)
        {
            _includeSections = includeSections;
        }

        public override OperationType OperationType => OperationType.Info;

        public DirectorySecurity Execute()
        {
            Journalize(this);

            if (_includeSections.HasValue)
            {
                return _directory.TxFileSystem.FileSystem.Directory.GetAccessControl(_path, _includeSections.Value);
            }

            return _directory.TxFileSystem.FileSystem.Directory.GetAccessControl(_path);
        }
    }
}
