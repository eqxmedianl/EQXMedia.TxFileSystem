namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Backups;
    using System;

    internal abstract class FileBackupOperation : IBackupOperation
    {
        private readonly ITxFile _file = null;
        protected readonly Guid _tempFileUuid;

        protected FileBackupOperation(ITxFile file, string path)
        {
            _file = file;
            _tempFileUuid = Guid.NewGuid();

            this.Path = path;
        }

        public string BackupPath
        {
            get
            {
                return ((TxFile)_file).GetBackupPath(this.Path, _tempFileUuid);
            }
        }

        public string Path { get; private set; }

        public abstract OperationType OperationType { get; }

        public virtual void Backup()
        {
            if (!OperationBackupGuard.ShouldBackup(((TxFile)_file)._txFileSystem.Journal, this.OperationType))
            {
                return;
            }

            if (!_file.FileSystem.File.Exists(this.BackupPath))
            {
                _file.FileSystem.File.Copy(this.Path, this.BackupPath);
            }
        }

        public void Delete()
        {
            if (_file.FileSystem.File.Exists(this.BackupPath))
            {
                _file.FileSystem.File.Delete(this.BackupPath);
            }
        }

        public void Restore()
        {
            if (_file.FileSystem.File.Exists(this.BackupPath))
            {
                _file.FileSystem.File.Copy(this.BackupPath, this.Path, overwrite: true);
            }
            Delete();
        }
    }
}