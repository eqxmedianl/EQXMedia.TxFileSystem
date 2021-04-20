namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional file info exposes methods to aid in the creation of <see cref="System.IO.FileInfo" /> 
    ///   objects. Which can be used to get information about files.
    ///   
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/summary/*' />
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     <c>TxFileInfo</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileInfo" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </para>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/remarks/*' />
    /// </remarks>
    [Serializable]
    public class TxFileInfo : IFileInfoFactory
    {
        internal TxFileInfo(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <summary>
        ///   Returns the <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> this <see 
        ///     cref="EQXMedia.TxFileSystem.TxFileInfo" /> instance belongs to. Thus not the actual file 
        ///     system being wrapped.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///      Use <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileSystem" /> to perform operations on 
        ///      the wrapped file system.
        ///   </para>
        /// </remarks>
        internal TxFileSystem TxFileSystem { get; set; }

        public IFileInfo FromFileName(string fileName)
        {
            return this.TxFileSystem.FileSystem.FileInfo.FromFileName(fileName);
        }
    }
}