namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.IO.Abstractions;

    internal interface ITxFileStream : IFileStreamFactory
    {
        public IFileSystem FileSystem { get; }

        internal ITxFileSystem TxFileSystem { get; set; }
    }
}