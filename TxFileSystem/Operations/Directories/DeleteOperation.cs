namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal class DeleteOperation : DirectoryOperation, IExecutingOperation
    {
        private readonly bool? _recursive = null;

        public DeleteOperation(ITxDirectory directory, string path)
            : base(directory, path)
        {
        }

        public DeleteOperation(ITxDirectory directory, string path, bool recursive)
            : this(directory, path)
        {
            _recursive = recursive;
        }

        public override OperationType OperationType => OperationType.Delete;

        public void Execute()
        {
            Journalize(this);

            if (!_recursive.HasValue)
            {
                _directory.TxFileSystem.FileSystem.Directory.Delete(_path);
            }
            else
            {
                _directory.TxFileSystem.FileSystem.Directory.Delete(_path, _recursive.Value);
            }
        }
    }
}
