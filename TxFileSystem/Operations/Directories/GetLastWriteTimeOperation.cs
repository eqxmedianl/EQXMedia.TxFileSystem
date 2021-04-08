namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class GetLastWriteTimeOperation : DirectoryOperation, IReturningOperation<DateTime>
    {
        private readonly bool _asUtc = false;

        public GetLastWriteTimeOperation(ITxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public GetLastWriteTimeOperation(ITxDirectory directory, string path, bool asUtc)
            : this(directory, path)
        {
            _asUtc = asUtc;
        }

        public override OperationType OperationType => OperationType.Info;

        public DateTime Execute()
        {
            Journalize(this);

            if (_asUtc)
            {
                return _directory.FileSystem.Directory.GetLastWriteTimeUtc(_path);
            }

            return _directory.FileSystem.Directory.GetLastWriteTime(_path);
        }
    }
}
