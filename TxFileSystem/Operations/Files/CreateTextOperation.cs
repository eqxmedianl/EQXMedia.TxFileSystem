namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class CreateTextOperation : FileOperation, IReturningOperation<StreamWriter>
    {
        public CreateTextOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Create;

        public StreamWriter Execute()
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.CreateText(_path);
        }

        public override void Rollback()
        {
            new DeleteOperation(_file, _path).Execute();
        }
    }
}
