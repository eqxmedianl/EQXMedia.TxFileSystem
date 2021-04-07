namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;

    internal sealed class SetAttributesOperation : FileOperation, IExecutingOperation
    {
        private readonly FileAttributes _fileAttributes;

        public SetAttributesOperation(ITxFile file, string path, FileAttributes fileAttributes)
            : base(file, path)
        {
            _fileAttributes = fileAttributes;
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            _file.TxFileSystem.FileSystem.File.SetAttributes(_path, _fileAttributes);
        }
    }
}
