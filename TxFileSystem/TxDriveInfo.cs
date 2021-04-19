namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <remarks>
    ///   <c>TxDriveInfo</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.DriveInfo" />. 
    ///   It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    /// </remarks>
    [Serializable]
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