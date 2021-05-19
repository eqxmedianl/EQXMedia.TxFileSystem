namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class EncryptOperation : FileOperation, IExecutingOperation
    {
        public EncryptOperation(TxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            _file.TxFileSystem.FileSystem.File.Encrypt(_path);
        }

        public override void Rollback()
        {
            new DecryptOperation(_file, _path).Execute();
        }
    }
}
