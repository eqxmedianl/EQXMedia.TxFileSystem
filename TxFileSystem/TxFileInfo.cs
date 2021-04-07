namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions;

    internal class TxFileInfo : IFileInfoFactory
    {
        public TxFileInfo(ITxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        public ITxFileSystem TxFileSystem { get; }

        public IFileInfo FromFileName(string fileName)
        {
            return this.TxFileSystem.FileSystem.FileInfo.FromFileName(fileName);
        }
    }
}