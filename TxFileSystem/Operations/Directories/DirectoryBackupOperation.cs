namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.IO.Extensions;
    using System;

    internal abstract class DirectoryBackupOperation : IBackupOperation
    {
        private readonly ITxDirectory _directory;
        private readonly Guid _tempFileUuid;

        protected DirectoryBackupOperation(ITxDirectory directory, string path)
        {
            _directory = directory;
            _tempFileUuid = Guid.NewGuid();

            this.Path = path;
        }

        public string BackupPath
        {
            get
            {
                var parentDirectory = _directory.FileSystem.DirectoryInfo.FromDirectoryName(this.Path)
                    .Parent.FullName + _directory.FileSystem.Path.DirectorySeparatorChar;
                var backupDir = parentDirectory + "tempdir_" + _tempFileUuid + "_" + _directory.FileSystem
                    .FileInfo.FromFileName(this.Path).Name;

                return backupDir;
            }
        }

        public bool IsBackedUp { get; private set; } = false;

        public abstract OperationType OperationType { get; }

        public string Path { get; private set; }

        public virtual void Backup()
        {
            if (!OperationBackupGuard.ShouldBackup(((TxDirectory)_directory)._txFileSystem.Journal, this.OperationType))
            {
                return;
            }

            if (_directory.FileSystem.Directory.Exists(this.Path))
            {
                ((TxDirectory)_directory).CopyRecursive(this.Path, this.BackupPath);
            }

            this.IsBackedUp = true;
        }

        public virtual void Delete()
        {
            if (!OperationBackupGuard.ShouldRestore(((TxDirectory)_directory)._txFileSystem.Journal, this.OperationType))
            {
                return;
            }

            if (_directory.FileSystem.Directory.Exists(this.BackupPath))
            {
                _directory.FileSystem.Directory.Delete(this.BackupPath, recursive: true);
            }
        }

        public virtual void Restore()
        {
            ((TxDirectory)_directory).CopyRecursive(this.BackupPath, this.Path);

            Delete();
        }
    }
}