namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional drive info exposes methods to aid in the creation of <see cref="System.IO.DriveInfo" /> 
    ///   objects. Which can be used to get information about drives.
    ///   
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/summary/*' />
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     <c>TxDriveInfo</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.DriveInfo" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </para>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/remarks/*' />
    /// </remarks>
    [Serializable]
    public sealed class TxDriveInfo : IDriveInfoFactory
    {
        internal TxDriveInfo(TxFileSystem fileSystem)
        {
            this.TxFileSystem = fileSystem;
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="TxFileSystem"]/*' />
        internal TxFileSystem TxFileSystem { get; set; }

        public IDriveInfo FromDriveName(string driveName)
        {
            return this.TxFileSystem.FileSystem.DriveInfo.FromDriveName(driveName);
        }

        /// <summary>
        ///   Returns information about the drives.
        /// </summary>
        /// <returns>The information about the drives.</returns>
        public IDriveInfo[] GetDrives()
        {
            return this.TxFileSystem.FileSystem.DriveInfo.GetDrives();
        }
    }
}