namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class OpenWriteOperation : FileOperation, IReturningOperation<Stream>
    {
        public OpenWriteOperation(TxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Open;

        public Stream Execute()
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.OpenWrite(_path);
        }
    }
}
