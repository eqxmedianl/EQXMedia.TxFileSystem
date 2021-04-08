namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions;

    internal sealed class TxDriveInfo : IDriveInfoFactory
    {
        public TxDriveInfo(TxFileSystem fileSystem)
        {
            this.TxFileSystem = fileSystem;
        }

        public TxFileSystem TxFileSystem { get; }

        public IDriveInfo FromDriveName(string driveName)
        {
            return this.TxFileSystem.FileSystem.DriveInfo.FromDriveName(driveName);
        }

        public IDriveInfo[] GetDrives()
        {
            return this.TxFileSystem.FileSystem.DriveInfo.GetDrives();
        }
    }
}