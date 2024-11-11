#if SYMBOLIC_LINKS

namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;
    using System.IO.Abstractions;

    internal sealed class CreateSymbolicLinkOperation : FileOperation, IReturningOperation<IFileSystemInfo>
    {
        private readonly bool _alreadyExisted;
        private readonly string _pathToTarget;

        public CreateSymbolicLinkOperation(TxFile file, string path, string pathToTarget)
            : base(file, path)
        {
            _alreadyExisted = file.Exists(path);
            _pathToTarget = pathToTarget;
        }

        public override OperationType OperationType => OperationType.CreateSymlink;

        public IFileSystemInfo Execute()
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.CreateSymbolicLink(_path, _pathToTarget);
        }

        public override void Restore()
        {
            if (!_alreadyExisted)
            {
                new DeleteOperation(_file, _path).Execute();
            }

            base.Restore();
        }
    }
}

#endif