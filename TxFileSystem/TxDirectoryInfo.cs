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

        public TxFileSystem TxFileSystem { get; }

        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return this.TxFileSystem.FileSystem.DirectoryInfo.FromDirectoryName(directoryName);
        }
    }
}