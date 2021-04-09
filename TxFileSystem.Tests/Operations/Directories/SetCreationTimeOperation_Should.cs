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

    public sealed class SetCreationTimeOperation_Should
    {
        [Fact]
        public void SetCreationTimeOperation_CreationTimeChanged()
        {
            var dirName = "/tmp/dirtochangecreationtimeof";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory(dirName);

            var originalCreationTime = txFileSystem.Directory.GetCreationTime(dirName);
            var modifiedCreationTime = originalCreationTime.AddDays(-7);

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.Directory.SetCreationTime(dirName, modifiedCreationTime);

                transactionScope.Complete();
            }

            Assert.Equal(modifiedCreationTime, txFileSystem.Directory.GetCreationTime(dirName));
        }

        [Fact]
        public void SetCreationTimeOperation_ExceptionThrown_CreationTimeUnchanged()
        {
            var dirName = "/tmp/dirtochangecreationtimeof";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory(dirName);

            var originalCreationTime = txFileSystem.Directory.GetCreationTime(dirName);
            var modifiedCreationTime = originalCreationTime.AddDays(-7);

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.Directory.SetCreationTime(dirName, modifiedCreationTime);

                    throw new Exception("Error occurred right after changing creation time");
                }
            });

            Assert.NotEqual(modifiedCreationTime, txFileSystem.Directory.GetCreationTime(dirName));
            Assert.Equal(originalCreationTime, txFileSystem.Directory.GetCreationTime(dirName));
        }

        [Fact, FsFact]
        public void SetCreationTimeOperation_AsUtc_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var dirName = "/tmp/dirtochangecreationtimeof";
            var dateTime = DateTime.UtcNow;

            fileSystemMock.Setup(f => f.Directory.SetCreationTimeUtc(It.Is<string>((s) => s == dirName),
                It.Is<DateTime>((d) => d == dateTime)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.Directory.SetCreationTimeUtc(dirName, dateTime);

            fileSystemMock.Verify(f => f.Directory.SetCreationTimeUtc(It.Is<string>((s) => s == dirName),
                It.Is<DateTime>((d) => d == dateTime)), Times.Once);
        }
    }
}
