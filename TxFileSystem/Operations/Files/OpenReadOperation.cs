namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class OpenReadOperation : FileOperation, IReturningOperation<Stream>
    {
        public OpenReadOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Read;

        public Stream Execute()
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.OpenRead(_path);
        }
    }
}
