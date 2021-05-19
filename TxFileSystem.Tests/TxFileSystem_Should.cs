namespace EQXMedia.TxFileSystem.Tests
{
    using EQXMedia.TxFileSystem.Exceptions;
    using System.IO.Abstractions;
    using Xunit;

    public sealed class TxFileSystem_Should
    {
        [Fact]
        public void TxFileSystem_FileSystemPassed_Internally_ReturnsWrappedFileSystem()
        {
            var txFileSystem = new TxFileSystem(new FileSystem());

            Assert.IsType<FileSystem>(txFileSystem.FileSystem);
        }

        [Fact]
        public void TxFileSystem_FileSystemPassed_Directory_FileSystem_ReturnsParentTxFileSystem()
        {
            var txFileSystem = new TxFileSystem(new FileSystem());

            Assert.IsType<TxFileSystem>(txFileSystem.Directory.FileSystem);
            Assert.Equal(txFileSystem, txFileSystem.Directory.FileSystem);
        }

        [Fact]
        public void TxFileSystem_FileSystemPassed_File_FileSystem_ReturnsParentTxFileSystem()
        {
            var txFileSystem = new TxFileSystem(new FileSystem());

            Assert.IsType<TxFileSystem>(txFileSystem.File.FileSystem);
            Assert.Equal(txFileSystem, txFileSystem.File.FileSystem);
        }

        [Fact]
        public void TxFileSystem_FileSystemPassed_FileStream_FileSystem_ReturnsParentTxFileSystem()
        {
            var txFileSystem = new TxFileSystem(new FileSystem());

            Assert.IsType<TxFileSystem>(txFileSystem.FileStream.FileSystem);
            Assert.Equal(txFileSystem, txFileSystem.FileStream.FileSystem);
        }

        [Fact]
        public void TxFileSystem_FileSystemPassed_Path_FileSystem_ReturnsParentTxFileSystem()
        {
            var txFileSystem = new TxFileSystem(new FileSystem());

            Assert.IsType<TxFileSystem>(txFileSystem.Path.FileSystem);
            Assert.Equal(txFileSystem, txFileSystem.Path.FileSystem);
        }

        [Fact]
        public void TxFileSystem_TxFileSystemPassed_ThrowsUnsupportedFileSystemImplementationException()
        {
            Assert.Throws<UnsupportedFileSystemImplementationException>(() =>
            {
                new TxFileSystem(new TxFileSystem());
            });
        }
    }
}
