﻿namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Backups;
    using System;

    internal abstract class FileBackupOperation : IBackupOperation
    {
        protected readonly TxFile _file = null;
        protected readonly Guid _tempFileUuid;

        protected FileBackupOperation(TxFile file, string path)
        {
            _file = file;
            _tempFileUuid = Guid.NewGuid();

            this.Path = path;
        }

        public string BackupPath
        {
            get
            {
                return _file.GetBackupPath(this.Path, _tempFileUuid);
            }
        }

        public string Path { get; private set; }

        public abstract OperationType OperationType { get; }

        public virtual void Backup()
        {
            if (!OperationBackupGuard.ShouldBackup(_file.TxFileSystem.Journal, this.OperationType))
            {
                return;
            }

            if (!_file.TxFileSystem.FileSystem.File.Exists(this.BackupPath))
            {
                _file.TxFileSystem.FileSystem.File.Copy(this.Path, this.BackupPath);
            }
        }

        public void Delete()
        {
            if (_file.TxFileSystem.FileSystem.File.Exists(this.BackupPath))
            {
                _file.TxFileSystem.FileSystem.File.Delete(this.BackupPath);
            }
        }

        public virtual void Restore()
        {
            if (_file.TxFileSystem.FileSystem.File.Exists(this.BackupPath))
            {
                _file.TxFileSystem.FileSystem.File.Copy(this.BackupPath, this.Path, overwrite: true);
            }
            Delete();
        }
    }
}