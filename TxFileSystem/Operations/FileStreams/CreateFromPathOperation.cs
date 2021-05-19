namespace EQXMedia.TxFileSystem.Operations.FileStreams
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class CreateFromPathOperation : FileStreamOperation, IReturningOperation<Stream>
    {
        private readonly FileAccess? _fileAccess;
        private readonly FileMode _fileMode;
        private readonly FileShare? _fileShare;
        private readonly int? _bufferSize;
        private readonly FileOptions? _fileOptions;
        private readonly bool? _useAsync;

        public CreateFromPathOperation(TxFileStream fileStream, string path, FileMode mode)
            : base(fileStream, path)
        {
            _fileMode = mode;
        }

        public CreateFromPathOperation(TxFileStream fileStream, string path, FileMode mode, FileAccess fileAccess)
            : this(fileStream, path, mode)
        {
            _fileAccess = fileAccess;
        }

        public CreateFromPathOperation(TxFileStream fileStream, string path, FileMode mode, FileAccess fileAccess,
            FileShare fileShare)
            : this(fileStream, path, mode, fileAccess)
        {
            _fileShare = fileShare;
        }

        public CreateFromPathOperation(TxFileStream fileStream, string path, FileMode mode, FileAccess fileAccess,
            FileShare fileShare, int bufferSize)
            : this(fileStream, path, mode, fileAccess, fileShare)
        {
            _bufferSize = bufferSize;
        }

        public CreateFromPathOperation(TxFileStream fileStream, string path, FileMode mode, FileAccess fileAccess,
            FileShare fileShare, int bufferSize, FileOptions options)
            : this(fileStream, path, mode, fileAccess, fileShare, bufferSize)
        {
            _fileOptions = options;
        }

        public CreateFromPathOperation(TxFileStream fileStream, string path, FileMode mode, FileAccess fileAccess,
            FileShare fileShare, int bufferSize, bool useAsync)
            : this(fileStream, path, mode, fileAccess, fileShare, bufferSize)
        {
            _useAsync = useAsync;
        }

        public override OperationType OperationType => OperationType.CreateFileStream;

        public Stream Execute()
        {
            Journalize(this);

            if (_fileAccess.HasValue && _fileShare.HasValue && _bufferSize.HasValue &&
                !_fileOptions.HasValue && _useAsync.HasValue)
            {
                _stream = _fileStream.TxFileSystem.FileSystem.FileStream.Create(_path, _fileMode,
                    _fileAccess.Value, _fileShare.Value, _bufferSize.Value, _useAsync.Value);
            }
            else if (_fileAccess.HasValue && _fileShare.HasValue && _bufferSize.HasValue &&
                _fileOptions.HasValue)
            {
                _stream = _fileStream.TxFileSystem.FileSystem.FileStream.Create(_path, _fileMode,
                    _fileAccess.Value, _fileShare.Value, _bufferSize.Value, _fileOptions.Value);
            }
            else if (_fileAccess.HasValue && _fileShare.HasValue && _bufferSize.HasValue)
            {
                _stream = _fileStream.TxFileSystem.FileSystem.FileStream.Create(_path, _fileMode,
                    _fileAccess.Value, _fileShare.Value, _bufferSize.Value);
            }
            else if (_fileAccess.HasValue && _fileShare.HasValue)
            {
                _stream = _fileStream.TxFileSystem.FileSystem.FileStream.Create(_path, _fileMode,
                    _fileAccess.Value, _fileShare.Value);
            }
            else if (_fileAccess.HasValue)
            {
                _stream = _fileStream.TxFileSystem.FileSystem.FileStream.Create(_path, _fileMode,
                    _fileAccess.Value);
            }
            else
            {
                _stream = _fileStream.TxFileSystem.FileSystem.FileStream.Create(_path, _fileMode);
            }

            Backup();

            return _stream;
        }
    }
}
