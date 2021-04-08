﻿namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Security.AccessControl;

#if NET5_0
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
#endif
    internal sealed class GetAccessControlOperation : FileOperation, IReturningOperation<FileSecurity>
    {
        private readonly AccessControlSections? _includeSections = null;

        public GetAccessControlOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public GetAccessControlOperation(ITxFile file, string path, AccessControlSections includeSections)
            : this(file, path)
        {
            _includeSections = includeSections;
        }

        public override OperationType OperationType => OperationType.Info;

        public FileSecurity Execute()
        {
            Journalize(this);

            if (_includeSections.HasValue)
            {
                return _file.TxFileSystem.FileSystem.File.GetAccessControl(_path, _includeSections.Value);
            }

            return _file.TxFileSystem.FileSystem.File.GetAccessControl(_path);
        }
    }
}
