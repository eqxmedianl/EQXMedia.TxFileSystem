namespace EQXMedia.TxFileSystem.Operations.FileStreams
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using Microsoft.Win32.SafeHandles;
    using System.IO;

    internal sealed class CreateFromSafeFileHandleOperation : FileStreamOperation, IReturningOperation<Stream>
    {
        private readonly FileAccess _access;
        private readonly int? _bufferSize;
        private readonly bool? _isAsync;

        public CreateFromSafeFileHandleOperation(ITxFileStream fileStream, SafeFileHandle handle, FileAccess access)
            : base(fileStream, handle)
        {
            _safeFileHandle = handle;
            _access = access;
        }

        public CreateFromSafeFileHandleOperation(ITxFileStream fileStream, SafeFileHandle handle, FileAccess access,
            int bufferSize)
            : this(fileStream, handle, access)
        {
            _bufferSize = bufferSize;
        }

        public CreateFromSafeFileHandleOperation(ITxFileStream fileStream, SafeFileHandle handle, FileAccess access,
            int bufferSize, bool isAsync)
            : this(fileStream, handle, access, bufferSize)
        {
            _isAsync = isAsync;
        }

        public override OperationType OperationType => OperationType.CreateFileStream;

        public Stream Execute()
        {
            Journalize(this);

            if (_bufferSize.HasValue && _isAsync.HasValue)
            {
                _stream = _fileStream.FileSystem.FileStream.Create(_safeFileHandle, _access,
                    _bufferSize.Value, _isAsync.Value);
            }
            else if (_bufferSize.HasValue)
            {
                _stream = _fileStream.FileSystem.FileStream.Create(_safeFileHandle, _access,
                    _bufferSize.Value);
            }
            else
            {
                _stream = _fileStream.FileSystem.FileStream.Create(_safeFileHandle, _access);
            }

            Backup();

            return _stream;
        }
    }
}
