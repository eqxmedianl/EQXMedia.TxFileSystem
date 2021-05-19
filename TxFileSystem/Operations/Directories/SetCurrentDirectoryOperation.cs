namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;

    internal sealed class SetCurrentDirectoryOperation : DirectoryOperation, IExecutingOperation
    {
        private readonly string _newDirectoryPath;

        public SetCurrentDirectoryOperation(TxDirectory directory, string path)
            : base(directory, GetCurrentDirectory(directory))
        {
            _newDirectoryPath = path;
        }

        private static string GetCurrentDirectory(TxDirectory directory)
        {
            return directory.TxFileSystem.FileSystem.Directory.GetCurrentDirectory();
        }

        public override OperationType OperationType => OperationType.Navigate;

        public void Execute()
        {
            Journalize(this);

            _directory.TxFileSystem.FileSystem.Directory.SetCurrentDirectory(_newDirectoryPath);
        }

        public override void Restore()
        {
            _directory.TxFileSystem.FileSystem.Directory.SetCurrentDirectory(this.Path);

            Delete();
        }

        public override void Backup()
        {
            // Don't backup anything because the old CWD is stored in a field variable.
        }
    }
}
