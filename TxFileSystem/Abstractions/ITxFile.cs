namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.IO.Abstractions;

    public interface ITxFile
    {
        IFileSystem FileSystem { get; }
    }
}
