namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class GetAttributesOperation : FileOperation, IReturningOperation<FileAttributes>
    {
        public GetAttributesOperation(TxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Info;

        public FileAttributes Execute()
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.GetAttributes(_path);
        }
    }
}
