namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class ExistsOperation : FileOperation, IReturningOperation<bool>
    {
        public ExistsOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Info;

        public bool Execute()
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.Exists(_path);
        }
    }
}
