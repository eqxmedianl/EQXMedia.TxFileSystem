#if SYMBOLIC_LINKS

namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions;

    internal sealed class CreateSymbolicLinkOperation : DirectoryOperation, IReturningOperation<IFileSystemInfo>
    {
        private readonly bool _alreadyExisted;
        private readonly string _pathToTarget;

        public CreateSymbolicLinkOperation(TxDirectory directory, string path, string pathToTarget)
            : base(directory, path)
        {
            _alreadyExisted = directory.TxFileSystem.File.Exists(path);
            _pathToTarget = pathToTarget;
        }

        public override OperationType OperationType => OperationType.CreateSymlink;

        public IFileSystemInfo Execute()
        {
            Journalize(this);

            return _directory.TxFileSystem.FileSystem.Directory.CreateSymbolicLink(_path, _pathToTarget);
        }

        public override void Restore()
        {
            if (!_alreadyExisted)
            {
                new DeleteOperation(_directory, _path).Execute();
            }

            base.Restore();
        }
    }
}

#endif