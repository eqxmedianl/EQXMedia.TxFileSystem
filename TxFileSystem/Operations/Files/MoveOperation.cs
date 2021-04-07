namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class MoveOperation : FileOperation, IExecutingOperation
    {
        private readonly string _destPath = null;

        public MoveOperation(ITxFile file, string path, string destPath)
            : base(file, path)
        {
            _destPath = destPath;
        }

        public override OperationType OperationType => OperationType.Move;

        public void Execute()
        {
            Journalize(this);

            _file.TxFileSystem.FileSystem.File.Move(_path, _destPath);
        }

        public override void Rollback()
        {
            new MoveOperation(_file, _destPath, _path).Execute();
        }
    }
}
