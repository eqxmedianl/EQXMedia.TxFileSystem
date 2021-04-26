namespace EQXMedia.TxFileSystem.Operations.FileStreams
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;
    using System.IO;

#if NET5_0
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
#endif
    [Obsolete("Even though the underlying FileStream.Create() methods are deprecated they are still part of the interface")]
    internal sealed class CreateFromFileHandleOperation : FileStreamOperation, IReturningOperation<Stream>
    {
        private readonly FileAccess _access;
        private readonly bool? _ownsHandle;
        private readonly int? _bufferSize;
        private readonly bool? _isAsync;

        public CreateFromFileHandleOperation(TxFileStream fileStream, IntPtr handle, FileAccess access)
            : base(fileStream, handle)
        {
            _handle = handle;
            _access = access;
        }

        public CreateFromFileHandleOperation(TxFileStream fileStream, IntPtr handle, FileAccess access,
            bool ownsHandle)
            : this(fileStream, handle, access)
        {
            _ownsHandle = ownsHandle;
        }

        public CreateFromFileHandleOperation(TxFileStream fileStream, IntPtr handle, FileAccess access,
            bool ownsHandle, int bufferSize)
            : this(fileStream, handle, access, ownsHandle)
        {
            _bufferSize = bufferSize;
        }

        public CreateFromFileHandleOperation(TxFileStream fileStream, IntPtr handle, FileAccess access,
            bool ownsHandle, int bufferSize, bool isAsync)
            : this(fileStream, handle, access, ownsHandle, bufferSize)
        {
            _isAsync = isAsync;
        }

        public override OperationType OperationType => OperationType.CreateFileStream;

        public Stream Execute()
        {
            Journalize(this);

            if (_ownsHandle.HasValue && _bufferSize.HasValue && _isAsync.HasValue)
            {
                _stream = _fileStream.TxFileSystem.FileSystem.FileStream.Create(_handle, _access,
                    _ownsHandle.Value, _bufferSize.Value, _isAsync.Value);
            }
            else if (_ownsHandle.HasValue && _bufferSize.HasValue)
            {
                _stream = _fileStream.TxFileSystem.FileSystem.FileStream.Create(_handle, _access,
                    _ownsHandle.Value, _bufferSize.Value);
            }
            else if (_ownsHandle.HasValue)
            {
                _stream = _fileStream.TxFileSystem.FileSystem.FileStream.Create(_handle, _access,
                    _ownsHandle.Value);
            }
            else
            {
                _stream = _fileStream.TxFileSystem.FileSystem.FileStream.Create(_handle, _access);
            }

            Backup();

            return _stream;
        }
    }
}
