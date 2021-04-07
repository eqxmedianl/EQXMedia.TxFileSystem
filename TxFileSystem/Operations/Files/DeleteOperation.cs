namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class DeleteOperation : FileOperation, IExecutingOperation
    {
        public DeleteOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Delete;

        public void Execute()
        {
            Journalize(this);

            _file.TxFileSystem.FileSystem.File.Delete(_path);
        }
    }
}
