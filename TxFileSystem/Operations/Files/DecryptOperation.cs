namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class DecryptOperation : FileOperation, IExecutingOperation
    {
        public DecryptOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            _file.FileSystem.File.Decrypt(_path);
        }

        public override void Rollback()
        {
            new EncryptOperation(_file, _path).Execute();
        }
    }
}
