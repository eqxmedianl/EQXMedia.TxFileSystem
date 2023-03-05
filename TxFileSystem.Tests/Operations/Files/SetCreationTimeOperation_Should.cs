namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.TestingHelpers.Attributes;
    using Moq;
    using System;
#if SUPPRESS_SIMPLE_USING
    using System.Diagnostics.CodeAnalysis;
#endif
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
            var fileName = "/tmp/filetochangecreationtimeof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);

            var originalCreationTime = txFileSystem.File.GetCreationTime(fileName);
            var modifiedCreationTime = originalCreationTime.AddDays(-7);

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.SetCreationTime(fileName, modifiedCreationTime);

                transactionScope.Complete();
            }

            Assert.Equal(modifiedCreationTime, txFileSystem.File.GetCreationTime(fileName));
        }

        [Fact]
#if SUPPRESS_SIMPLE_USING
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void SetCreationTimeOperation_ExceptionThrown_CreationTimeUnchanged()
        {
            var fileName = "/tmp/filetochangecreationtimeof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);

            var originalCreationTime = txFileSystem.File.GetCreationTime(fileName);
            var modifiedCreationTime = originalCreationTime.AddDays(-7);

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.File.SetCreationTime(fileName, modifiedCreationTime);

                    throw new Exception("Error occurred right after changing creation time");
                }
            });

            Assert.NotEqual(modifiedCreationTime, txFileSystem.File.GetCreationTime(fileName));
            Assert.Equal(originalCreationTime.Year, txFileSystem.Directory.GetCreationTime(fileName).Year);
            Assert.Equal(originalCreationTime.Month, txFileSystem.Directory.GetCreationTime(fileName).Month);
            Assert.Equal(originalCreationTime.Day, txFileSystem.Directory.GetCreationTime(fileName).Day);
            Assert.Equal(originalCreationTime.Hour, txFileSystem.Directory.GetCreationTime(fileName).Hour);
            Assert.Equal(originalCreationTime.Minute, txFileSystem.Directory.GetCreationTime(fileName).Minute);
            Assert.Equal(originalCreationTime.Second, txFileSystem.Directory.GetCreationTime(fileName).Second);
        }

        [Fact, FsFact]
        public void SetCreationTimeOperation_AsUtc_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetochangecreationtimeof.txt";
            var dateTime = DateTime.UtcNow;

            fileSystemMock.Setup(f => f.File.SetCreationTimeUtc(It.Is<string>((s) => s == fileName),
                It.Is<DateTime>((d) => d == dateTime)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.SetCreationTimeUtc(fileName, dateTime);

            fileSystemMock.Verify(f => f.File.SetCreationTimeUtc(It.Is<string>((s) => s == fileName),
                It.Is<DateTime>((d) => d == dateTime)), Times.Once);
        }
    }
}
