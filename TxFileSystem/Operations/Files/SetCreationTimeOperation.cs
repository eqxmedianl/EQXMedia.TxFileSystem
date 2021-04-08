namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class SetCreationTimeOperation : FileOperation, IExecutingOperation
    {
        private readonly bool _asUtc = false;
        private readonly DateTime _creationTime;

        public SetCreationTimeOperation(ITxFile file, string path, DateTime creationTime)
            : base(file, path)
        {
            _creationTime = creationTime;
        }

        public SetCreationTimeOperation(ITxFile file, string path, DateTime creationTime, bool asUtc)
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
                _file.TxFileSystem.FileSystem.File.SetCreationTimeUtc(_path, _creationTime);

                return;
            }

            _file.TxFileSystem.FileSystem.File.SetCreationTime(_path, _creationTime);
        }
    }
}
