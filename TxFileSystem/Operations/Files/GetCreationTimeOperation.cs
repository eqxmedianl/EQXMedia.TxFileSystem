namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal sealed class GetCreationTimeOperation : FileOperation, IReturningOperation<DateTime>
    {
        private readonly bool _asUtc;

        public GetCreationTimeOperation(ITxFile file, string path, bool asUtc = false)
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
                return _file.TxFileSystem.FileSystem.File.GetCreationTimeUtc(_path);
            }

            return _file.TxFileSystem.FileSystem.File.GetCreationTime(_path);
        }
    }
}
