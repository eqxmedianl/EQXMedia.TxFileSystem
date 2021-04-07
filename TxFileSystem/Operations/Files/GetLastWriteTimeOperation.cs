namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class GetLastWriteTimeOperation : FileOperation, IReturningOperation<DateTime>
    {
        private readonly bool _asUtc;

        public GetLastWriteTimeOperation(ITxFile file, string path, bool asUtc = false)
            : base(file, path)
        {
            _asUtc = asUtc;
        }

        public override OperationType OperationType => OperationType.Info;

        public DateTime Execute()
        {
            Journalize(this);

            if (_asUtc)
            {
                return _file.TxFileSystem.FileSystem.File.GetLastWriteTimeUtc(_path);
            }

            return _file.TxFileSystem.FileSystem.File.GetLastWriteTime(_path);
        }
    }
}
