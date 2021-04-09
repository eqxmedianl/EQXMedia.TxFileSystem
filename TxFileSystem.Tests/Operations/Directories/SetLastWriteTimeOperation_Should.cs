namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Reflection;
    using System.Transactions;
    using Xunit;

    public sealed class SetLastWriteTimeOperation_Should
    {
        [Fact]
        public void SetLastWriteTimeOperation_ResultsTimeModified()
        {
            var dirName = "/tmp";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            txFileSystem.Directory.CreateDirectory(dirName);
            var originalWriteTime = txFileSystem.Directory.GetLastWriteTime(dirName);
            var modifiedWriteTime = DateTime.Now.AddDays(-7);

            txFileSystem.Directory.SetLastWriteTime(dirName, modifiedWriteTime);

            Assert.NotEqual(originalWriteTime, txFileSystem.Directory.GetLastWriteTime(dirName));
            Assert.Equal(modifiedWriteTime, txFileSystem.Directory.GetLastWriteTime(dirName));
        }

        [Fact]
        public void SetLastWriteTimeOperation_ExceptionThrown_ResultsTimeNotChanged()
        {
            var dirName = "/tmp";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            txFileSystem.Directory.CreateDirectory(dirName);
            var originalWriteTime = txFileSystem.Directory.GetLastWriteTime(dirName);
            var modifiedWriteTime = DateTime.Now.AddDays(-7);

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);

                    txFileSystem.Directory.SetLastWriteTime(dirName, modifiedWriteTime);

                    throw new Exception("Exception occurred right after modifying write time");
                }
            });

            Assert.Equal(originalWriteTime, txFileSystem.Directory.GetLastWriteTime(dirName));
            Assert.NotEqual(modifiedWriteTime, txFileSystem.Directory.GetLastWriteTime(dirName));
        }

        [Fact, FsFact]
        public void SetLastWriteTimeOperation_AsUtc_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var dirName = "/tmp";
            var dateTime = DateTime.UtcNow;

            fileSystemMock.Setup(f => f.Directory.SetLastWriteTimeUtc(It.Is<string>((s) => s == dirName),
                It.Is<DateTime>((d) => d == dateTime)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.Directory.SetLastWriteTimeUtc(dirName, dateTime);

            fileSystemMock.Verify(f => f.Directory.SetLastWriteTimeUtc(It.Is<string>((s) => s == dirName),
                It.Is<DateTime>((d) => d == dateTime)), Times.Once);
        }
    }
}
