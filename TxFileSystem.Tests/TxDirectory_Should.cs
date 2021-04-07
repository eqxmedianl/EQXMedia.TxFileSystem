namespace EQXMedia.TxFileSystem.Tests
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using Xunit;

    public sealed class TxDirectory_Should
    {
        [Fact]
        public void TxDirectory_FileSystem_SameFileSystemReturned()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            Assert.Equal(mockFileSystem, ((ITxDirectory)txFileSystem.Directory).FileSystem);
        }
    }
}
