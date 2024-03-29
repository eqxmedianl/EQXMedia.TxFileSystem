﻿namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions;

    internal sealed class GetParentOperation : DirectoryOperation, IReturningOperation<IDirectoryInfo>
    {
        public GetParentOperation(TxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public override OperationType OperationType => OperationType.Info;

        public IDirectoryInfo Execute()
        {
            Journalize(this);

            return _directory.TxFileSystem.FileSystem.Directory.GetParent(_path);
        }
    }
}
