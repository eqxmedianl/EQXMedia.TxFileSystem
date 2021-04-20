namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;

#if NET5_0
    using global::EQXMedia.TxFileSystem.Backups;
#endif

    internal sealed class MoveOperation : FileOperation, IExecutingOperation
    {
        private readonly string _destPath = null;
#if NET5_0
        private readonly bool? _overwrite;
#endif

        public MoveOperation(TxFile file, string path, string destPath)
            : base(file, path)
        {
            _destPath = destPath;
        }

#if NET5_0
        public MoveOperation(TxFile file, string path, string destPath, bool overwrite)
            : base(file, path)
        {
            _destPath = destPath;
            _overwrite = overwrite;

            Backup();
        }
#endif

        public override OperationType OperationType => OperationType.Move;

#if NET5_0
        public override void Backup()
        {
            base.Backup();

            if (_overwrite.HasValue && _overwrite.Value && _file.TxFileSystem.FileSystem.File.Exists(_destPath))
            {
                _file.TxFileSystem.FileSystem.File.Copy(_destPath, ((TxFile)_file).GetBackupPath(_destPath, _tempFileUuid));
            }
        }
#endif

        public void Execute()
        {
            Journalize(this);

#if NET5_0
            if (_overwrite.HasValue)
            {
                _file.TxFileSystem.FileSystem.File.Move(_path, _destPath, _overwrite.Value);

                return;
            }
#endif
            _file.TxFileSystem.FileSystem.File.Move(_path, _destPath);
        }

        public override void Rollback()
        {
            new MoveOperation(_file, _destPath, _path).Execute();

#if NET5_0
            RestoreOverwrittenDestinationFile();
#endif
        }

#if NET5_0
        private void RestoreOverwrittenDestinationFile()
        {
            var backupFile = ((TxFile)_file).GetBackupPath(_destPath, _tempFileUuid);
            if (_overwrite.HasValue && _overwrite.Value && _file.TxFileSystem.FileSystem.File.Exists(backupFile))
            {
                _file.TxFileSystem.FileSystem.File.Copy(backupFile, _destPath);
                _file.TxFileSystem.FileSystem.File.Delete(backupFile);
            }
        }
#endif
    }
}
