namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions;
    using System.Security.AccessControl;

    internal sealed class CreateDirectoryOperation : DirectoryOperation, IReturningOperation<IDirectoryInfo>
    {
        private readonly DirectorySecurity _directorySecurity = null;
        private bool _alreadyExisted = false;

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

            if (_directory.TxFileSystem.FileSystem.Directory.Exists(_path))
            {
                _alreadyExisted = true;
            }

            if (_directorySecurity != null)
            {
                return _directory.FileSystem.Directory.CreateDirectory(_path, _directorySecurity);
            }

            return _directory.FileSystem.Directory.CreateDirectory(_path);
        }

        public override void Restore()
        {
            if (!_alreadyExisted)
            {
                new DeleteOperation(_directory, _path, recursive: true).Execute();
            }

            Delete();
        }
    }
}
