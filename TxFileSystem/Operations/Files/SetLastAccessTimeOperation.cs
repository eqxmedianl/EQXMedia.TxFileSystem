namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class SetLastAccessTimeOperation : FileOperation, IExecutingOperation
    {
        private readonly bool _asUtc = false;
        private readonly DateTime _lastAccessTime;

        public SetLastAccessTimeOperation(ITxFile file, string path, DateTime lastAccessTime)
            : base(file, path)
        {
            _lastAccessTime = lastAccessTime;
        }

        public SetLastAccessTimeOperation(ITxFile file, string path, DateTime creationTime, bool asUtc)
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
                _file.FileSystem.File.SetLastAccessTimeUtc(_path, _lastAccessTime);
            }
            else
            {
                _file.FileSystem.File.SetLastAccessTime(_path, _lastAccessTime);
            }
        }
    }
}
