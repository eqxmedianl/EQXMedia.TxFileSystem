namespace EQXMedia.TxFileSystem.Abstractions
{
    using global::EQXMedia.TxFileSystem.Journaling;
    using System.IO.Abstractions;

    public interface ITxFileSystem : IFileSystem
    {
        internal IFileSystem FileSystem { get; set; }

        internal TxJournal Journal { get; }
    }
}