namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.IO;

    internal abstract class FileStreamBackupOperation : IBackupOperation
    {
        protected readonly TxFileStream _fileStream = null;
        protected readonly string _path = null;

        private string _backupPath;
        private readonly Guid _tempFileUuid;

        protected IntPtr _handle = IntPtr.Zero;
        protected SafeFileHandle _safeFileHandle = null;

        protected Stream _stream;

        protected FileStreamBackupOperation(TxFileStream fileStream, string path)
        {
            _fileStream = fileStream;
            _path = path;

            _tempFileUuid = Guid.NewGuid();
        }

        protected FileStreamBackupOperation(TxFileStream fileStream, IntPtr handle, string path)
        {
            _fileStream = fileStream;
            _handle = handle;
            _path = path;

            _tempFileUuid = Guid.NewGuid();
        }

        protected FileStreamBackupOperation(TxFileStream fileStream, SafeFileHandle safeFileHandle)
        {
            _fileStream = fileStream;
            _safeFileHandle = safeFileHandle;

            _tempFileUuid = Guid.NewGuid();
        }

        public string BackupPath
        {
            get
            {
                if (_backupPath != null)
                {
                    return _backupPath;
                }

                if (_safeFileHandle == null && _path != null)
                {
                    var backupDirectory = _fileStream.TxFileSystem.FileSystem.FileInfo.FromFileName(_path).DirectoryName + _fileStream.TxFileSystem.FileSystem.Path.DirectorySeparatorChar;
                    _backupPath = backupDirectory + "temp_" + _tempFileUuid + "_" + _fileStream.TxFileSystem.FileSystem.FileInfo.FromFileName(_path).Name;
                }
                else
                {
                    _backupPath = _fileStream.TxFileSystem.FileSystem.Path.GetTempFileName();
                }

                return _backupPath;
            }
        }

        public abstract OperationType OperationType { get; }

        public virtual void Backup()
        {
            if (!OperationBackupGuard.ShouldBackup(_fileStream.TxFileSystem.Journal, this.OperationType))
            {
                return;
            }

            var oldPosition = _stream.Position;

            var data = new byte[_stream.Length];
            _stream.Seek(0, SeekOrigin.Begin);
            _stream.Read(data, 0, (int)_stream.Length);

            var backupFileStream = _fileStream.TxFileSystem.FileSystem.File.Create(this.BackupPath);
            backupFileStream.Write(data, 0, data.Length);
            backupFileStream.Flush();
            backupFileStream.Close();

            _stream.Seek(oldPosition, SeekOrigin.Begin);
        }

        public void Delete()
        {
            if (_fileStream.TxFileSystem.FileSystem.File.Exists(this.BackupPath))
            {
                _fileStream.TxFileSystem.FileSystem.File.Delete(this.BackupPath);
            }
        }

        public void Restore()
        {
            if (_fileStream.TxFileSystem.FileSystem.File.Exists(this.BackupPath))
            {
                var originalData = _fileStream.TxFileSystem.FileSystem.File.ReadAllBytes(this.BackupPath);

                _stream.Seek(0, SeekOrigin.Begin);
                _stream.SetLength(originalData.Length);
                _stream.Write(originalData, 0, originalData.Length);
                _stream.Flush();
            }
            Delete();
        }
    }
}