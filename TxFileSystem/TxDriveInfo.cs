namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <remarks>
    ///   <c>TxDriveInfo</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.DriveInfo" />. 
    ///   It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    /// </remarks>
    [Serializable]
    public sealed class TxDriveInfo : IDriveInfoFactory
    {
        internal TxDriveInfo(TxFileSystem fileSystem)
        {
            this.TxFileSystem = fileSystem;
        }

        /// <summary>
        ///   Returns the <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> this <see 
        ///     cref="EQXMedia.TxFileSystem.TxDriveInfo" /> instance belongs to. Thus not the actual file 
        ///     system being wrapped.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///      Use <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileSystem" /> to perform operations on 
        ///      the wrapped file system.
        ///   </para>
        /// </remarks>
        internal TxFileSystem TxFileSystem { get; set; }

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