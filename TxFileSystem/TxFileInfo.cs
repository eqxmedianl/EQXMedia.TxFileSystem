namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <remarks>
    ///   <c>TxFileInfo</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileInfo" />. 
    ///   It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    /// </remarks>
    [Serializable]
    public class TxFileInfo : IFileInfoFactory
    {
        internal TxFileInfo(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        public TxFileSystem TxFileSystem { get; }

        public IFileInfo FromFileName(string fileName)
        {
            return this.TxFileSystem.FileSystem.FileInfo.FromFileName(fileName);
        }
    }
}