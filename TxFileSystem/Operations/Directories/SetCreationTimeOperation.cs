namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class SetCreationTimeOperation : DirectoryOperation, IExecutingOperation
    {
        private readonly bool _asUtc = false;
        private readonly DateTime _creationTime;

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

        public void Execute()
        {
            Journalize(this);

            if (_asUtc)
            {
                _directory.TxFileSystem.FileSystem.Directory.SetCreationTimeUtc(_path, _creationTime);
            }

            _directory.TxFileSystem.FileSystem.Directory.SetCreationTime(_path, _creationTime);
        }

        public override void Rollback()
        {
            Restore();
        }
    }
}
