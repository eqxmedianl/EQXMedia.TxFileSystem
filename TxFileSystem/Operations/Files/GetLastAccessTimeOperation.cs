﻿namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class GetLastAccessTimeOperation : FileOperation, IReturningOperation<DateTime>
    {
        private readonly bool _asUtc = false;

        public GetLastAccessTimeOperation(TxFile file, string path)
            : base(file, path)
        {
        }

        public GetLastAccessTimeOperation(TxFile file, string path, bool asUtc)
            : this(file, path)
        {
            _asUtc = asUtc;
        }

        public override OperationType OperationType => OperationType.Info;

        public DateTime Execute()
        {
            Journalize(this);

            if (_asUtc)
            {
                return _file.TxFileSystem.FileSystem.File.GetLastAccessTimeUtc(_path);
            }

            return _file.TxFileSystem.FileSystem.File.GetLastAccessTime(_path);
        }
    }
}
