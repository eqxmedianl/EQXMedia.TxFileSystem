namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal class ReplaceOperation : FileOperation, IExecutingOperation
    {
        private readonly string _destPath = null;
        private readonly string _destBackupFileName = null;
        private readonly bool? _ignoreMetadataErrors = null;

        public ReplaceOperation(ITxFile file, string path, string destPath, string destPathBackupFileName)
            : base(file, path)
        {
            _destPath = destPath;
            _destBackupFileName = destPathBackupFileName;
        }

        public ReplaceOperation(ITxFile file, string path, string destPath, string destPathBackupFileName,
            bool ignoreMetadataErrors)
            : this(file, path, destPath, destPathBackupFileName)
        {
            _ignoreMetadataErrors = ignoreMetadataErrors;
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            if (!_ignoreMetadataErrors.HasValue)
            {
                _file.TxFileSystem.FileSystem.File.Replace(_path, _destPath, _destBackupFileName);
            }
            else
            {
                _file.TxFileSystem.FileSystem.File.Replace(_path, _destPath, _destBackupFileName, _ignoreMetadataErrors.Value);
            }
        }

        public override void Rollback()
        {
            new MoveOperation(_file, _destPath, _path).Execute();
            if (new ExistsOperation(_file, _destBackupFileName).Execute())
            {
                new MoveOperation(_file, _destBackupFileName, _destPath).Execute();
            }
        }
    }
}
