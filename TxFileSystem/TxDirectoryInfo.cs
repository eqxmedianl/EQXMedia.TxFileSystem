namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions;

    internal sealed class TxDirectoryInfo : IDirectoryInfoFactory
    {
        public TxDirectoryInfo(ITxFileSystem fileSystem)
        {
            this.TxFileSystem = fileSystem;
        }

        public ITxFileSystem TxFileSystem { get; }

        public IDirectoryInfo FromDirectoryName(string directoryName)
        {
            return this.TxFileSystem.FileSystem.DirectoryInfo.FromDirectoryName(directoryName);
        }
    }
}