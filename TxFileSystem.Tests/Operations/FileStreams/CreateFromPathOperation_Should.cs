namespace EQXMedia.TxFileSystem.Tests.Operations.FileStreams
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using global::EQXMedia.TxFileSystem.Tests.Operations.FileStreams.Utils;
    using Moq;
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Text;
    using System.Transactions;
    using Xunit;

    public sealed class CreateFromPathOperation_Should
    {
        [Fact]
        public void CreateFromPathOperation_Open_ContentsChanged()
        {
            var fileSystem = new FileSystem();
            var txFileSystem = new TxFileSystem(fileSystem);

            string fileName = UnitTestUtils.GetTempFileName(txFileSystem);

            var streamWriter = txFileSystem.File.CreateText(fileName);
            streamWriter.Write("Original contents" + Environment.NewLine);
            streamWriter.Flush();
            streamWriter.Close();

            using var transactionScope = new TransactionScope();

            txFileSystem = new TxFileSystem(fileSystem);
            var fileStream = txFileSystem.FileStream.Create(fileName, FileMode.Open);
            var data = Encoding.Default.GetBytes("Written" + Environment.NewLine);
            fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
            fileStream.SetLength(fileStream.Length + data.Length);
            fileStream.Write(data, 0, data.Length);
            fileStream.Flush();

            transactionScope.Complete();

            fileStream.Close();

            Assert.Equal("Original contents" + Environment.NewLine
                + "Written" + Environment.NewLine, txFileSystem.File.ReadAllText(fileName));

            txFileSystem.File.Delete(fileName);
        }

        [Fact]
        public void CreateFromPathHandleOperation_Open_ExceptionThrown_ContentsUnchanged()
        {
            var fileSystem = new FileSystem();
            var txFileSystem = new TxFileSystem(fileSystem);

            string fileName = UnitTestUtils.GetTempFileName(txFileSystem);

            var streamWriter = txFileSystem.File.CreateText(fileName);
            streamWriter.Write("Original contents" + Environment.NewLine);
            streamWriter.Flush();
            streamWriter.Close();

            Stream fileStream = null;
            var data = Encoding.Default.GetBytes("Written" + Environment.NewLine);

            Assert.ThrowsAny<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();

                txFileSystem = new TxFileSystem(fileSystem);
                fileStream = txFileSystem.FileStream.Create(fileName, FileMode.Open);
                fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
                fileStream.SetLength(fileStream.Length + data.Length);
                fileStream.Write(data, 0, data.Length);
                fileStream.Flush();

                throw new Exception("Error occurred right after writing to stream");
            });

            fileStream.Close();

            Assert.Equal("Original contents" + Environment.NewLine, txFileSystem.File.ReadAllText(fileName));

            txFileSystem.File.Delete(fileName);
        }

        [Fact, FsFact]
        public void CreateFromPathOperation_FileModeAndFileAccess_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fileName = UnitTestUtils.GetTempFileName(txFileSystem);
            var fileMode = FileMode.OpenOrCreate;
            var fileAccess = FileAccess.ReadWrite;

            var fileStreamMock = new Mock<FileStream>(MockBehavior.Loose, fileName, fileMode, fileAccess);

            fileSystemMock.Setup(f => f.FileStream.Create(It.Is<string>((f) => f == fileName),
                It.Is<FileMode>((m) => m == fileMode), It.Is<FileAccess>((a) => a == fileAccess)))
                .Returns(fileStreamMock.Object);

            var fileStreamReturned = txFileSystem.FileStream.Create(fileName, fileMode, fileAccess);

            fileSystemMock.Verify(f => f.FileStream.Create(It.Is<string>((f) => f == fileName),
                It.Is<FileMode>((m) => m == fileMode), It.Is<FileAccess>((a) => a == fileAccess)), Times.Once);

            Assert.Equal(fileStreamMock.Object, fileStreamReturned);
        }

        [Fact, FsFact]
        public void CreateFromPathOperation_FileModeAndFileAccessAndFileShare_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fileName = UnitTestUtils.GetTempFileName(txFileSystem);
            var fileMode = FileMode.OpenOrCreate;
            var fileAccess = FileAccess.ReadWrite;
            var fileShare = FileShare.Read;

            var fileStreamMock = new Mock<FileStream>(MockBehavior.Loose, fileName, fileMode, fileAccess, fileShare);

            fileSystemMock.Setup(f => f.FileStream.Create(It.Is<string>((f) => f == fileName),
                It.Is<FileMode>((m) => m == fileMode), It.Is<FileAccess>((a) => a == fileAccess),
                It.Is<FileShare>((s) => s == fileShare)))
                .Returns(fileStreamMock.Object);

            var fileStreamReturned = txFileSystem.FileStream.Create(fileName, fileMode, fileAccess, fileShare);

            fileSystemMock.Verify(f => f.FileStream.Create(It.Is<string>((f) => f == fileName),
                It.Is<FileMode>((m) => m == fileMode), It.Is<FileAccess>((a) => a == fileAccess),
                It.Is<FileShare>((s) => s == fileShare)), Times.Once);

            Assert.Equal(fileStreamMock.Object, fileStreamReturned);
        }

        [Fact, FsFact]
        public void CreateFromPathOperation_FileModeAndFileAccessAndFileShareAndBufferSize_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fileName = UnitTestUtils.GetTempFileName(txFileSystem);
            var fileMode = FileMode.OpenOrCreate;
            var fileAccess = FileAccess.ReadWrite;
            var fileShare = FileShare.Read;
            var bufferSize = 512;

            var fileStreamMock = new Mock<FileStream>(MockBehavior.Loose, fileName, fileMode, fileAccess, fileShare,
                bufferSize);

            fileSystemMock.Setup(f => f.FileStream.Create(It.Is<string>((f) => f == fileName),
                It.Is<FileMode>((m) => m == fileMode), It.Is<FileAccess>((a) => a == fileAccess),
                It.Is<FileShare>((s) => s == fileShare), It.Is<int>((s) => s == bufferSize)))
                .Returns(fileStreamMock.Object);

            var fileStreamReturned = txFileSystem.FileStream.Create(fileName, fileMode, fileAccess,
                fileShare, bufferSize);

            fileSystemMock.Verify(f => f.FileStream.Create(It.Is<string>((f) => f == fileName),
                It.Is<FileMode>((m) => m == fileMode), It.Is<FileAccess>((a) => a == fileAccess),
                It.Is<FileShare>((s) => s == fileShare), It.Is<int>((s) => s == bufferSize)), Times.Once);

            Assert.Equal(fileStreamMock.Object, fileStreamReturned);
        }

        [Fact, FsFact]
        public void CreateFromPathOperation_FileModeAndFileAccessAndFileShareAndBufferSizeAndUseAsync_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fileName = UnitTestUtils.GetTempFileName(txFileSystem);
            var fileMode = FileMode.OpenOrCreate;
            var fileAccess = FileAccess.ReadWrite;
            var fileShare = FileShare.Read;
            var useAsync = false;
            var bufferSize = 512;

            var fileStreamMock = new Mock<FileStream>(MockBehavior.Loose, fileName, fileMode, fileAccess, fileShare,
                bufferSize, useAsync);

            fileSystemMock.Setup(f => f.FileStream.Create(It.Is<string>((f) => f == fileName),
                It.Is<FileMode>((m) => m == fileMode), It.Is<FileAccess>((a) => a == fileAccess),
                It.Is<FileShare>((s) => s == fileShare), It.Is<int>((s) => s == bufferSize),
                It.Is<bool>((a) => a == useAsync)))
                .Returns(fileStreamMock.Object);

            var fileStreamReturned = txFileSystem.FileStream.Create(fileName, fileMode, fileAccess,
                fileShare, bufferSize, useAsync);

            fileSystemMock.Verify(f => f.FileStream.Create(It.Is<string>((f) => f == fileName),
                It.Is<FileMode>((m) => m == fileMode), It.Is<FileAccess>((a) => a == fileAccess),
                It.Is<FileShare>((s) => s == fileShare), It.Is<int>((s) => s == bufferSize),
                It.Is<bool>((a) => a == useAsync)), Times.Once);

            Assert.Equal(fileStreamMock.Object, fileStreamReturned);
        }

        [Fact, FsFact]
        public void CreateFromPathOperation_FileModeAndFileAccessAndFileShareAndBufferSizeAndFileOptions_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fileName = UnitTestUtils.GetTempFileName(txFileSystem);
            var fileMode = FileMode.OpenOrCreate;
            var fileAccess = FileAccess.ReadWrite;
            var fileShare = FileShare.Read;
            var bufferSize = 512;
            var fileOptions = FileOptions.SequentialScan;

            var fileStreamMock = new Mock<FileStream>(MockBehavior.Loose, fileName, fileMode, fileAccess, fileShare,
                bufferSize, fileOptions);

            fileSystemMock.Setup(f => f.FileStream.Create(It.Is<string>((f) => f == fileName),
                It.Is<FileMode>((m) => m == fileMode), It.Is<FileAccess>((a) => a == fileAccess),
                It.Is<FileShare>((s) => s == fileShare), It.Is<int>((s) => s == bufferSize),
                It.Is<FileOptions>((o) => o == fileOptions)))
                .Returns(fileStreamMock.Object);

            var fileStreamReturned = txFileSystem.FileStream.Create(fileName, fileMode, fileAccess,
                fileShare, bufferSize, fileOptions);

            fileSystemMock.Verify(f => f.FileStream.Create(It.Is<string>((f) => f == fileName),
                It.Is<FileMode>((m) => m == fileMode), It.Is<FileAccess>((a) => a == fileAccess),
                It.Is<FileShare>((s) => s == fileShare), It.Is<int>((s) => s == bufferSize),
                It.Is<FileOptions>((o) => o == fileOptions)), Times.Once);

            Assert.Equal(fileStreamMock.Object, fileStreamReturned);
        }
    }
}
