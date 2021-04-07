namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class GetLastAccessTimeOperation : DirectoryOperation, IReturningOperation<DateTime>
    {
        private readonly bool _asUtc = false;

        public GetLastAccessTimeOperation(ITxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public GetLastAccessTimeOperation(ITxDirectory directory, string path, bool asUtc)
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
                return _directory.TxFileSystem.FileSystem.Directory.GetLastAccessTimeUtc(_path);
            }

            return _directory.TxFileSystem.FileSystem.Directory.GetLastAccessTime(_path);
        }
    }
}
