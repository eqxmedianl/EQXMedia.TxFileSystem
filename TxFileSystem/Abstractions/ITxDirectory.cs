namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.IO.Abstractions;

    public interface ITxDirectory
    {
        public IFileSystem FileSystem { get; }

        internal ITxFileSystem TxFileSystem { get; set; }
    }
}
