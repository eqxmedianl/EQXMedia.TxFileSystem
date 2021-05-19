namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class SetLastAccessTimeOperation : DirectoryOperation, IExecutingOperation
    {
        private readonly bool _asUtc = false;
        private readonly DateTime _lastAccessTime;
        private DateTime _oldLastAccessTimeUtc;
        private DateTime _oldLastAccessTime;

        public SetLastAccessTimeOperation(TxDirectory directory, string path, DateTime lastAcccesTime)
            : base(directory, path)
        {
            _lastAccessTime = lastAcccesTime;
        }

        public SetLastAccessTimeOperation(TxDirectory directory, string path, DateTime lastAccessTime, bool asUtc)
            : this(directory, path, lastAccessTime)
        {
            _asUtc = asUtc;
        }

        public override OperationType OperationType => OperationType.ModifyInfo;

        public override void Backup()
        {
            _oldLastAccessTimeUtc = _directory.TxFileSystem.FileSystem.Directory.GetLastAccessTimeUtc(this.Path);
            _oldLastAccessTime = _directory.TxFileSystem.FileSystem.Directory.GetLastAccessTime(this.Path);
        }

        public void Execute()
        {
            Journalize(this);

            if (_asUtc)
            {
                _directory.TxFileSystem.FileSystem.Directory.SetLastAccessTimeUtc(_path, _lastAccessTime);
            }

            _directory.TxFileSystem.FileSystem.Directory.SetLastAccessTime(_path, _lastAccessTime);
        }

        public override void Restore()
        {
            if (_asUtc)
            {
                _directory.TxFileSystem.FileSystem.Directory.SetLastAccessTimeUtc(_path, _oldLastAccessTimeUtc);
            }
            else
            {
                _directory.TxFileSystem.FileSystem.Directory.SetLastAccessTime(_path, _oldLastAccessTime);
            }

            Delete();
        }
    }
}
