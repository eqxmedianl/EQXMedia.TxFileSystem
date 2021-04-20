namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class DecryptOperation : FileOperation, IExecutingOperation
    {
        public DecryptOperation(TxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            _file.TxFileSystem.FileSystem.File.Decrypt(_path);
        }

        public override void Rollback()
        {
            new EncryptOperation(_file, _path).Execute();
        }
    }
}
