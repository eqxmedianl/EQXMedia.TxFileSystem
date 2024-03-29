﻿namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class AppendTextOperation : FileOperation, IReturningOperation<StreamWriter>
    {
        public AppendTextOperation(TxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Modify;

        public StreamWriter Execute()
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.AppendText(_path);
        }
    }
}
