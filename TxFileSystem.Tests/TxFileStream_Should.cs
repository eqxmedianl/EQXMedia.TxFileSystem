namespace EQXMedia.TxFileSystem.Tests
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using Xunit;

    public sealed class TxFileStream_Should
    {
        [Fact]
        public void TxFileStream_FileSystem_SameFileSystemReturned()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            Assert.Equal(mockFileSystem, txFileSystem.FileStream.TxFileSystem.FileSystem);
        }
    }
}
