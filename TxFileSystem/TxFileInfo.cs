namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional file info exposes methods to aid in the creation of <see cref="System.IO.FileInfo" /> 
    ///   objects. Which can be used to get information about files.
    /// </summary>
    /// <remarks>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/remarks/*' />
    ///   <note type="note">
    ///     <c>TxFileInfo</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileInfo" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </note>
    /// </remarks>
    [Serializable]
    public class TxFileInfo : IFileInfoFactory
    {
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@context="constructor" and @param="TxFileSystem"]/*' />
        internal TxFileInfo(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="TxFileSystem"]/*' />
        internal TxFileSystem TxFileSystem { get; set; }

        /// <summary>
        ///   Returns information about the file at the specified path.
        /// </summary>
        /// <param name="fileName">The path of the file to get information from.</param>
        /// <returns>The information about the file.</returns>
        public IFileInfo FromFileName(string fileName)
        {
            return this.TxFileSystem.FileSystem.FileInfo.FromFileName(fileName);
        }
    }
}