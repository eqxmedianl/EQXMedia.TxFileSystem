namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class SetLastWriteTimeOperation : FileOperation, IExecutingOperation
    {
        private readonly bool _asUtc = false;
        private readonly DateTime _lastWriteTime;

        public SetLastWriteTimeOperation(ITxFile file, string path, DateTime lastWriteTime)
            : base(file, path)
        {
            _lastWriteTime = lastWriteTime;
        }

        public SetLastWriteTimeOperation(ITxFile file, string path, DateTime creationTime, bool asUtc)
            : this(file, path, creationTime)
        {
            _asUtc = asUtc;
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            if (_asUtc)
            {
                _file.FileSystem.File.SetLastWriteTimeUtc(_path, _lastWriteTime);
            }
            else
            {
                _file.FileSystem.File.SetLastWriteTime(_path, _lastWriteTime);
            }
        }
    }
}
