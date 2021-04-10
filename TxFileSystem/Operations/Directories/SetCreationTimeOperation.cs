namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class SetCreationTimeOperation : DirectoryOperation, IExecutingOperation
    {
        private readonly bool _asUtc = false;
        private readonly DateTime _creationTime;
        private DateTime _oldCreationTimeUtc;
        private DateTime _oldCreationTime;

        public SetCreationTimeOperation(ITxDirectory directory, string path, DateTime creationTime)
            : base(directory, path)
        {
            _creationTime = creationTime;
        }

        public SetCreationTimeOperation(ITxDirectory directory, string path, DateTime creationTime, bool asUtc)
            : this(directory, path, creationTime)
        {
            _asUtc = asUtc;
        }

        public override OperationType OperationType => OperationType.Modify;

        public override void Backup()
        {
            _oldCreationTimeUtc = _directory.FileSystem.Directory.GetCreationTimeUtc(this.Path);
            _oldCreationTime = _directory.FileSystem.Directory.GetCreationTime(this.Path);
        }

        public void Execute()
        {
            Journalize(this);

            if (_asUtc)
            {
                _directory.FileSystem.Directory.SetCreationTimeUtc(_path, _creationTime);
            }

            _directory.FileSystem.Directory.SetCreationTime(_path, _creationTime);
        }

        public override void Restore()
        {
            if (_asUtc)
            {
                _directory.FileSystem.Directory.SetCreationTimeUtc(_path, _oldCreationTimeUtc);
            }
            else
            {
                _directory.FileSystem.Directory.SetCreationTime(_path, _oldCreationTime);
            }

            Delete();
        }
    }
}
