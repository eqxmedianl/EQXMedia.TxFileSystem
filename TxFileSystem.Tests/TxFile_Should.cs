namespace EQXMedia.TxFileSystem.Tests
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using Xunit;

    public sealed class TxFile_Should
    {
        [Fact]
        public void TxFile_FileSystem_SameFileSystemReturned()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            Assert.Equal(mockFileSystem, ((ITxFile)txFileSystem.File).FileSystem);
        }
    }
}
