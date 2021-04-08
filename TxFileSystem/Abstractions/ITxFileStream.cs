namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.IO.Abstractions;

    internal interface ITxFileStream : IFileStreamFactory
    {
        IFileSystem FileSystem { get; }
    }
}