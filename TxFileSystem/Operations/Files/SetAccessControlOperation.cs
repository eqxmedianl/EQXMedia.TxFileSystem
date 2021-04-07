namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Security.AccessControl;

    internal sealed class SetAccessControlOperation : FileOperation, IExecutingOperation
    {
        private readonly FileSecurity _fileSecurity;

        public SetAccessControlOperation(ITxFile file, string path, FileSecurity fileSecurity)
            : base(file, path)
        {
            _fileSecurity = fileSecurity;
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            _file.TxFileSystem.FileSystem.File.SetAccessControl(_path, _fileSecurity);
        }
    }
}
