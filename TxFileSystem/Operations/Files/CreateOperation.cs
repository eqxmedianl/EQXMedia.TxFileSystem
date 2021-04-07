namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class CreateOperation : FileOperation, IReturningOperation<Stream>
    {
        private readonly int? _bufferSize;
        private readonly FileOptions? _fileOptions;

        public CreateOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public CreateOperation(ITxFile file, string path, int bufferSize)
            : this(file, path)
        {
            _bufferSize = bufferSize;
        }

        public CreateOperation(ITxFile file, string path, int bufferSize, FileOptions options)
            : this(file, path, bufferSize)
        {
            _fileOptions = options;
        }

        public override OperationType OperationType => OperationType.Create;

        public Stream Execute()
        {
            Journalize(this);

            if (_fileOptions.HasValue)
            {
                return _file.TxFileSystem.FileSystem.File.Create(_path, _bufferSize.Value, _fileOptions.Value);
            }

            if (_bufferSize.HasValue)
            {
                return _file.TxFileSystem.FileSystem.File.Create(_path, _bufferSize.Value);
            }

            return _file.TxFileSystem.FileSystem.File.Create(_path);
        }

        public override void Rollback()
        {
            new DeleteOperation(_file, _path).Execute();
        }
    }
}
