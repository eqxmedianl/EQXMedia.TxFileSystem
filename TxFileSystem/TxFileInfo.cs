namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions;

    internal class TxFileInfo : IFileInfoFactory
    {
        public TxFileInfo(TxFileSystem txFileSystem)
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