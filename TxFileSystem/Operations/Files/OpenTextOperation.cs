namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class OpenTextOperation : FileOperation, IReturningOperation<StreamReader>
    {
        public OpenTextOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Read;

        public StreamReader Execute()
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.OpenText(_path);
        }
    }
}
