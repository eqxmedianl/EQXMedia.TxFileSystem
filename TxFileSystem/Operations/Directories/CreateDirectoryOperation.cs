namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions;
    using System.Security.AccessControl;

    internal sealed class CreateDirectoryOperation : DirectoryOperation, IReturningOperation<IDirectoryInfo>
    {
        private readonly DirectorySecurity _directorySecurity = null;

        public CreateDirectoryOperation(ITxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public CreateDirectoryOperation(ITxDirectory directory, string path, DirectorySecurity directorySecurity)
            : this(directory, path)
        {
            _directorySecurity = directorySecurity;
        }

        public override OperationType OperationType => OperationType.Create;

        public IDirectoryInfo Execute()
        {
            Journalize(this);

            if (_directorySecurity != null)
            {
                return _directory.FileSystem.Directory.CreateDirectory(_path, _directorySecurity);
            }

            return _directory.FileSystem.Directory.CreateDirectory(_path);
        }

        public override void Restore()
        {
            new DeleteOperation(_directory, _path).Execute();

            Delete();
        }
    }
}
