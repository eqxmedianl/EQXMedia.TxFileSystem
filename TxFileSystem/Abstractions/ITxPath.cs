namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.IO.Abstractions;

    public interface ITxPath : IPath
    {
        internal ITxFileSystem TxFileSystem { get; set; }
    }
}
