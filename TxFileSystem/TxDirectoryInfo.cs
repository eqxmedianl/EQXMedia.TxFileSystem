namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional directory info exposes methods to aid in the creation of <see cref="System.IO.DirectoryInfo" /> 
    ///   objects. Which can be used to get information about directories.
    /// </summary>
    /// <remarks>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/remarks/*' />
    ///   <note type="note">
    ///     <c>TxDirectoryInfo</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.DirectoryInfo" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </note>
    /// </remarks>
    [Serializable]
    public sealed class TxDirectoryInfo : IDirectoryInfoFactory
    {
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@context="constructor" and @param="TxFileSystem"]/*' />
        internal TxDirectoryInfo(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="TxFileSystem"]/*' />
        internal TxFileSystem TxFileSystem { get; set; }

        /// <summary>
        ///   Returns information about the directory at the specified path.
        /// </summary>
        /// <param name="directoryName">The path of the directory to get information from.</param>
        /// <returns>The information about the directory.</returns>
        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return this.TxFileSystem.FileSystem.DirectoryInfo.FromDirectoryName(directoryName);
        }
    }
}