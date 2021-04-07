namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Security.AccessControl;

    internal sealed class SetAccessControlOperation : DirectoryOperation, IExecutingOperation
    {
        private readonly DirectorySecurity _directorySecurity;

        public SetAccessControlOperation(ITxDirectory directory, string path, DirectorySecurity directorySecurity)
            : base(directory, path)
        {
            _directorySecurity = directorySecurity;
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            _directory.TxFileSystem.FileSystem.Directory.SetAccessControl(_path, _directorySecurity);
        }

        public override void Rollback()
        {
            var oldDirectorySecurity = _directory.TxFileSystem.FileSystem.Directory.GetAccessControl(this.BackupPath);
            _directory.TxFileSystem.FileSystem.Directory.SetAccessControl(_path, oldDirectorySecurity);

            Delete();
        }
    }
}
