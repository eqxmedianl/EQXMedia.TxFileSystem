namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class OpenWriteOperation : FileOperation, IReturningOperation<Stream>
    {
        public OpenWriteOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Open;

        public Stream Execute()
        {
            Journalize(this);

            return _file.FileSystem.File.OpenWrite(_path);
        }
    }
}
