namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class SetLastWriteTimeOperation : DirectoryOperation, IExecutingOperation
    {
        private readonly bool _asUtc = false;
        private readonly DateTime _lastWriteTime;

        public SetLastWriteTimeOperation(ITxDirectory directory, string path, DateTime lastWriteTime)
            : base(directory, path)
        {
            _lastWriteTime = lastWriteTime;
        }

        public SetLastWriteTimeOperation(ITxDirectory directory, string path, DateTime lastWriteTime, bool asUtc)
            : this(directory, path, lastWriteTime)
        {
            _asUtc = asUtc;
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            if (_asUtc)
            {
                _directory.TxFileSystem.FileSystem.Directory.SetLastWriteTimeUtc(_path, _lastWriteTime);
            }

            _directory.TxFileSystem.FileSystem.Directory.SetLastWriteTime(_path, _lastWriteTime);
        }
    }
}
