namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
#if NETCOREAPP3_1_OR_GREATER
    using System.Diagnostics.CodeAnalysis;
#endif
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
            var fileName = "/tmp/filetochangeaccesstimeof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);
            var originalAccessTime = txFileSystem.File.GetLastAccessTime(fileName);
            var modifiedAccessTime = DateTime.Now.AddDays(-7);

            txFileSystem.File.SetLastAccessTime(fileName, modifiedAccessTime);

            Assert.NotEqual(originalAccessTime, txFileSystem.File.GetLastAccessTime(fileName));
            Assert.Equal(modifiedAccessTime, txFileSystem.File.GetLastAccessTime(fileName));
        }

        [Fact]
#if NETCOREAPP3_1_OR_GREATER
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void SetLastAccessTimeOperation_ExceptionThrown_ResultsTimeNotChanged()
        {
            var fileName = "/tmp/filetochangeaccesstimeof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);
            var originalAccessTime = txFileSystem.File.GetLastAccessTime(fileName);
            var modifiedAccessTime = DateTime.Now.AddDays(-7);

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);

                    txFileSystem.File.SetLastAccessTime(fileName, modifiedAccessTime);

                    throw new Exception("Exception occurred right after modifying write time");
                }
            });

            Assert.Equal(originalAccessTime, txFileSystem.File.GetLastAccessTime(fileName));
            Assert.NotEqual(modifiedAccessTime, txFileSystem.File.GetLastAccessTime(fileName));
        }

        [Fact, FsFact]
        public void SetLastAccessTimeOperation_AsUtc_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetochangeaccesstimeof.txt";
            var dateTime = DateTime.UtcNow;

            fileSystemMock.Setup(f => f.File.SetLastAccessTimeUtc(It.Is<string>((s) => s == fileName),
                It.Is<DateTime>((d) => d == dateTime)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.SetLastAccessTimeUtc(fileName, dateTime);

            fileSystemMock.Verify(f => f.File.SetLastAccessTimeUtc(It.Is<string>((s) => s == fileName),
                It.Is<DateTime>((d) => d == dateTime)), Times.Once);
        }
    }
}
