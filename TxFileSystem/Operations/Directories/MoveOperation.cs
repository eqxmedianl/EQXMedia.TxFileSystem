namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class MoveOperation : DirectoryOperation, IExecutingOperation
    {
        private readonly string _destPath;

        public MoveOperation(ITxDirectory directory, string path, string destPath)
            : base(directory, path)
        {
            _destPath = destPath;
        }

        public override OperationType OperationType => OperationType.Move;

        public void Execute()
        {
            Journalize(this);

            _directory.TxFileSystem.FileSystem.Directory.Move(_path, _destPath);
        }

        public override void Rollback()
        {
            new MoveOperation(_directory, _destPath, _path).Execute();
        }
    }
}
