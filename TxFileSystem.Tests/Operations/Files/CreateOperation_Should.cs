namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using Moq;
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class CreateOperation_Should
    {
        [Fact]
        public void CreateOperation_FileExists()
        {
            var fileName = "/var/log/logtocreate.log";

            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/log");
            txFileSystem.File.Create(fileName);

            transactionScope.Complete();

            Assert.True(txFileSystem.File.Exists(fileName));
        }

        [Fact]
        public void CreateOperation_ExceptionThrown_FileDoesntExists()
        {
            var fileName = "/var/log/logtocreate.log";

            var mockFileSystem = new MockFileSystem();
            TxFileSystem txFileSystem = null;

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();
                
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.Directory.CreateDirectory("/var/log");
                txFileSystem.File.Create(fileName);

                throw new Exception("Error occured right after creating the file");
            });

            Assert.False(txFileSystem.File.Exists(fileName));
        }

        [Fact]
        public void CreateOperation_ReturnsStream()
        {
            var fileName = "/var/log/logtocreate.log";

            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/log");
            var createdStream = txFileSystem.File.Create(fileName);

            transactionScope.Complete();

            Assert.IsAssignableFrom<Stream>(createdStream);
        }

        [Fact]
        public void CreateOperation_Calls_CreateWithBufferSize_CalledOnce_ReturnsStream()
        {
            var fileName = "/var/log/logtocreate.log";
            var byteSize = 4096;
            var streamMock = new Mock<Stream>();

            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(f => f.File.Create(It.Is<string>((f) => f == fileName),
                It.Is<int>(i => i == byteSize)))
                .Returns(streamMock.Object);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var streamReturned = txFileSystem.File.Create(fileName, byteSize);

            fileSystemMock.Verify(f => f.File.Create(It.Is<string>((f) => f == fileName),
                It.Is<int>(i => i == byteSize)), Times.Once);

            Assert.IsAssignableFrom<Stream>(streamReturned);
            Assert.Equal(streamMock.Object, streamReturned);
        }

        [Fact]
        public void CreateOperation_Calls_CreateWithBufferSizeAndFileOptions_CalledOnce()
        {
            var fileName = "/var/log/logtocreate.log";
            var byteSize = 4096;
            var fileOptions = FileOptions.DeleteOnClose;

            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(f => f.File.Create(It.Is<string>((f => f == fileName)), It.Is<int>(i => i == byteSize),
                It.Is<FileOptions>(o => o == fileOptions)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.Create(fileName, byteSize, fileOptions);

            fileSystemMock.Verify(f => f.File.Create(It.Is<string>((f => f == fileName)), It.Is<int>(i => i == byteSize),
                It.Is<FileOptions>(o => o == fileOptions)), Times.Once);
        }
    }
}
