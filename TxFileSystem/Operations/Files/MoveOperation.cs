namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class MoveOperation : FileOperation, IExecutingOperation
    {
        private readonly string _destPath = null;
#if NET5_0
        private readonly bool _overwrite = false;
#endif

        public MoveOperation(ITxFile file, string path, string destPath)
            : base(file, path)
        {
            _destPath = destPath;
        }

#if NET5_0
        public MoveOperation(ITxFile file, string path, string destPath, bool overwrite)
            : this(file, path, destPath)
        {
            _overwrite = overwrite;
        }
#endif

        public override OperationType OperationType => OperationType.Move;

        public void Execute()
        {
            Journalize(this);

#if NET5_0
            _file.TxFileSystem.FileSystem.File.Move(_path, _destPath, _overwrite);
#else
            _file.TxFileSystem.FileSystem.File.Move(_path, _destPath);
#endif
        }

        public override void Rollback()
        {
            new MoveOperation(_file, _destPath, _path).Execute();
        }
    }
}
