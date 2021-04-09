namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Reflection;
    using System.Text;
    using System.Transactions;
    using Xunit;

    public sealed class OpenOperation_Should
    {
        [Fact]
        public void OpenOperation_RetunsStream()
        {
            var fileName = "/tmp/filetoopen.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);

            var fileStream = txFileSystem.File.Open(fileName, FileMode.Append);

            Assert.IsAssignableFrom<Stream>(fileStream);
        }

        [Fact]
        public void OpenOperation_AddedToJournal()
        {
            var fileName = "/tmp/filetoopen.txt";

            var mockFileSystem = new MockFileSystem();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.Open(fileName, FileMode.Append);
            }

            Assert.NotEmpty(txFileSystem.Journal._txJournalEntries);
        }

        [Fact]
        public void OpenOperation_BytesAreAppended()
        {
            var fileName = "/tmp/filetoopen.txt";
            var data = Encoding.UTF8.GetBytes("Text to append to file");

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);

            Stream fileStream = null;

            using (var transactionScope = new TransactionScope())
            {
                fileStream = txFileSystem.File.Open(fileName, FileMode.Append);
                fileStream.Write(data, 0, data.Length);
                fileStream.Flush();
                fileStream.Close();

                transactionScope.Complete();
            }

            Assert.Equal(data, txFileSystem.File.ReadAllBytes(fileName));
        }

        [Fact]
        public void OpenOperation_ExceptionThrown_BytesNotAppended()
        {
            var fileName = "/tmp/filetoopen.txt";
            var data = Encoding.UTF8.GetBytes("Text to append to file");

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);

            Assert.ThrowsAny<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);

                    var fileStream = txFileSystem.File.Open(fileName, FileMode.Append);
                    fileStream.Write(data, 0, data.Length);
                    fileStream.Flush();
                    fileStream.Close();

                    throw new Exception("Error occurred right after appending bytes");
                }
            });

            Assert.Equal(new byte[] { }, txFileSystem.File.ReadAllBytes(fileName));
        }

        [Fact, FsFact]
        public void OpenOperation_WithFileAccess_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetoopen.txt";

            fileSystemMock.Setup(f => f.File.Open(It.Is<string>((s) => s == fileName),
                It.Is<FileMode>((m) => m == FileMode.Open), It.Is<FileAccess>((a) => a == FileAccess.ReadWrite)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.Open(fileName, FileMode.Open, FileAccess.ReadWrite);

            fileSystemMock.Verify(f => f.File.Open(It.Is<string>((s) => s == fileName),
                It.Is<FileMode>((m) => m == FileMode.Open), It.Is<FileAccess>((a) => a == FileAccess.ReadWrite)),
                Times.Once);
        }

        [Fact, FsFact]
        public void OpenOperation_WithFileAccessAndFileShare_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetoopen.txt";

            fileSystemMock.Setup(f => f.File.Open(It.Is<string>((s) => s == fileName),
                It.Is<FileMode>((m) => m == FileMode.Open), It.Is<FileAccess>((a) => a == FileAccess.ReadWrite),
                It.Is<FileShare>((s) => s == FileShare.Delete)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete);

            fileSystemMock.Verify(f => f.File.Open(It.Is<string>((s) => s == fileName),
                It.Is<FileMode>((m) => m == FileMode.Open), It.Is<FileAccess>((a) => a == FileAccess.ReadWrite),
                It.Is<FileShare>((s) => s == FileShare.Delete)), Times.Once);
        }
    }
}
