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

    public sealed class SetLastAccessTimeOperation_Should
    {
        [Fact]
        public void SetLastAccessTimeOperation_ResultsTimeModified()
        {
            var dirName = "/tmp/dirtochangeaccesstimeof";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.Directory.CreateDirectory(dirName);
            var originalAccessTime = txFileSystem.Directory.GetLastAccessTime(dirName);
            var modifiedAccessTime = DateTime.Now.AddDays(-7);

            txFileSystem.Directory.SetLastAccessTime(dirName, modifiedAccessTime);

            Assert.NotEqual(originalAccessTime, txFileSystem.Directory.GetLastAccessTime(dirName));
            Assert.Equal(modifiedAccessTime, txFileSystem.Directory.GetLastAccessTime(dirName));
        }

        [Fact]
        public void SetLastAccessTimeOperation_ExceptionThrown_ResultsTimeNotChanged()
        {
            var dirName = "/tmp/dirtochangeaccesstimeof";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.Directory.CreateDirectory(dirName);
            var originalAccessTime = txFileSystem.Directory.GetLastAccessTime(dirName);
            var modifiedAccessTime = DateTime.Now.AddDays(-7);

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();
                var txFileSystem = new TxFileSystem(mockFileSystem);

                txFileSystem.Directory.SetLastAccessTime(dirName, modifiedAccessTime);

                throw new Exception("Exception occurred right after modifying write time");
            });

            Assert.Equal(originalAccessTime, txFileSystem.Directory.GetLastAccessTime(dirName));
            Assert.NotEqual(modifiedAccessTime, txFileSystem.Directory.GetLastAccessTime(dirName));
        }

        [Fact]
        public void SetLastAccessTimeOperation_AsUtc_ExceptionThrown_ResultsTimeNotChanged()
        {
            var dirName = "/tmp/dirtochangeaccesstimeof";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.Directory.CreateDirectory(dirName);
            var originalAccessTime = txFileSystem.Directory.GetLastAccessTimeUtc(dirName);
            var modifiedAccessTime = DateTime.UtcNow.AddDays(-7);

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();
                var txFileSystem = new TxFileSystem(mockFileSystem);

                txFileSystem.Directory.SetLastAccessTimeUtc(dirName, modifiedAccessTime);

                throw new Exception("Exception occurred right after modifying write time as utc");
            });

            Assert.Equal(originalAccessTime, txFileSystem.Directory.GetLastAccessTimeUtc(dirName));
            Assert.NotEqual(modifiedAccessTime, txFileSystem.Directory.GetLastAccessTimeUtc(dirName));
        }

        [Fact, FsFact]
        public void SetLastAccessTimeOperation_AsUtc_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var dirName = "/tmp/dirtochangeaccesstimeof";
            var dateTime = DateTime.UtcNow;

            fileSystemMock.Setup(f => f.Directory.SetLastAccessTimeUtc(It.Is<string>((s) => s == dirName),
                It.Is<DateTime>((d) => d == dateTime)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.Directory.SetLastAccessTimeUtc(dirName, dateTime);

            fileSystemMock.Verify(f => f.Directory.SetLastAccessTimeUtc(It.Is<string>((s) => s == dirName),
                It.Is<DateTime>((d) => d == dateTime)), Times.Once);
        }
    }
}
