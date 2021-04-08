namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.IO.Abstractions;

    public interface ITxDirectory
    {
        IFileSystem FileSystem { get; }
    }
}
