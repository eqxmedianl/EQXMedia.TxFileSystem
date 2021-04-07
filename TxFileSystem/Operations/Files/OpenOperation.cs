namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class OpenOperation : FileOperation, IReturningOperation<Stream>
    {
        private readonly FileMode _mode;
        private readonly FileAccess? _access;
        private readonly FileShare? _share;

        public OpenOperation(ITxFile file, string path, FileMode mode)
            : base(file, path)
        {
            _mode = mode;
        }

        public OpenOperation(ITxFile file, string path, FileMode mode, FileAccess access)
            : this(file, path, mode)
        {
            _access = access;
        }

        public OpenOperation(ITxFile file, string path, FileMode mode, FileAccess access, FileShare share)
            : this(file, path, mode, access)
        {
            _share = share;
        }

        public override OperationType OperationType => OperationType.Open;

        public Stream Execute()
        {
            Journalize(this);

            if (_access.HasValue && _share.HasValue)
            {
                return _file.TxFileSystem.FileSystem.File.Open(_path, _mode, _access.Value, _share.Value);
            }

            if (_access.HasValue)
            {
                return _file.TxFileSystem.FileSystem.File.Open(_path, _mode, _access.Value);
            }

            return _file.TxFileSystem.FileSystem.File.Open(_path, _mode);
        }
    }
}
