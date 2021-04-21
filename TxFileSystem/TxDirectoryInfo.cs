﻿namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional directory info exposes methods to aid in the creation of <see cref="System.IO.DirectoryInfo" /> 
    ///   objects. Which can be used to get information about directories.
    ///   
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/summary/*' />
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     <c>TxDirectoryInfo</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.DirectoryInfo" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </para>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/remarks/*' />
    /// </remarks>
    [Serializable]
    public sealed class TxDirectoryInfo : IDirectoryInfoFactory
    {
        internal TxDirectoryInfo(TxFileSystem fileSystem)
        {
            this.TxFileSystem = fileSystem;
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