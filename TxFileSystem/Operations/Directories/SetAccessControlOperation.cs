namespace EQXMedia.TxFileSystem.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Security.AccessControl;

#if NET5_0
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
#endif
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

            _directory.FileSystem.Directory.SetAccessControl(_path, _directorySecurity);
        }

        public override void Restore()
        {
            var oldDirectorySecurity = _directory.FileSystem.Directory.GetAccessControl(this.BackupPath);
            _directory.FileSystem.Directory.SetAccessControl(_path, oldDirectorySecurity);

            Delete();
        }
    }
}
