namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional drive info exposes methods to aid in the creation of <see cref="System.IO.DriveInfo" /> 
    ///   objects. Which can be used to get information about drives.
    /// </summary>
    /// <remarks>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/remarks/*' />
    ///   <note type="note">
    ///     <c>TxDriveInfo</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.DriveInfo" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </note>
    /// </remarks>
    [Serializable]
    public sealed class TxDriveInfo : IDriveInfoFactory
    {
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@context="constructor" and @param="TxFileSystem"]/*' />
        internal TxDriveInfo(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="TxFileSystem"]/*' />
        internal TxFileSystem TxFileSystem { get; set; }

        /// <summary>
        ///   Returns information about the drive with the specified name.
        /// </summary>
        /// <param name="driveName">The name of the drive to retrieve information from.</param>
        /// <returns>The information about the drive with the specified name.</returns>
        /// <seealso cref="System.IO.DriveInfo" alt="Resembles System.IO.Abstractions.IDriveInfo" target="_blank"/>
        public IDriveInfo FromDriveName(string driveName)
        {
            return this.TxFileSystem.FileSystem.DriveInfo.FromDriveName(driveName);
        }

        /// <summary>
        ///   Returns information about the drives.
        /// </summary>
        /// <returns>The information about the drives.</returns>
        /// <seealso cref="System.IO.DriveInfo" alt="Resembles System.IO.Abstractions.IDriveInfo" target="_blank"/>
        public IDriveInfo[] GetDrives()
        {
            return this.TxFileSystem.FileSystem.DriveInfo.GetDrives();
        }
    }
}