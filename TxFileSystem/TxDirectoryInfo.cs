namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;
    using System.IO.Abstractions;

    /// <remarks>
    ///   <c>TxDirectoryInfo</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.DirectoryInfo" />. 
    ///   It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    /// </remarks>
    [Serializable]
    public sealed class TxDirectoryInfo : IDirectoryInfoFactory
    {
        internal TxDirectoryInfo(TxFileSystem fileSystem)
        {
            this.TxFileSystem = fileSystem;
        }

        /// <summary>
        ///   Returns the <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> this <see 
        ///     cref="EQXMedia.TxFileSystem.TxDirectoryInfo" /> instance belongs to. Thus not the actual file 
        ///     system being wrapped.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///      Use <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileSystem" /> to perform operations on the 
        ///      wrapped file system.
        ///   </para>
        /// </remarks>
        internal TxFileSystem TxFileSystem { get; set; }

        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return this.TxFileSystem.FileSystem.DirectoryInfo.FromDirectoryName(directoryName);
        }
    }
}