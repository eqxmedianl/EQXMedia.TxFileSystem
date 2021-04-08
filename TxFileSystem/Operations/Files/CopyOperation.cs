namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Backups;

    internal sealed class CopyOperation : FileOperation, IExecutingOperation
    {
        private readonly string _destPath = null;
        private readonly bool? _overwrite = null;

        public CopyOperation(ITxFile file, string path, string destPath)
            : base(file, path)
        {
            _destPath = destPath;
        }

        public CopyOperation(ITxFile file, string path, string destPath, bool overwrite)
            : base(file, path)
        {
            _destPath = destPath;
            _overwrite = overwrite;

            Backup();
        }

        public override OperationType OperationType => OperationType.Duplicate;

        public override void Backup()
        {
            base.Backup();

            if (_overwrite.HasValue && _overwrite.Value && _file.FileSystem.File.Exists(_destPath))
            {
                _file.FileSystem.File.Copy(_destPath, _file.GetBackupPath(_destPath, _tempFileUuid));
            }
        }

        public void Execute()
        {
            Journalize(this);

            if (!_overwrite.HasValue)
            {
                _file.FileSystem.File.Copy(_path, _destPath);
            }
            else
            {
                _file.FileSystem.File.Copy(_path, _destPath, _overwrite.Value);
            }
        }

        public override void Rollback()
        {
            new DeleteOperation(_file, _destPath).Execute();

            RestoreOverwrittenDestinationFile();
        }

        private void RestoreOverwrittenDestinationFile()
        {
            var backupFile = _file.GetBackupPath(_destPath, _tempFileUuid);
            if (_overwrite.HasValue && _overwrite.Value && _file.FileSystem.File.Exists(backupFile))
            {
                _file.FileSystem.File.Copy(backupFile, _destPath);
                _file.FileSystem.File.Delete(backupFile);
            }
        }
    }
}
