namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
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

    public sealed class SetLastWriteTimeOperation_Should
    {
        [Fact]
        public void SetLastWriteTimeOperation_ResultsTimeModified()
        {
            var fileName = "/tmp/filetochangewritetimeof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);
            var originalWriteTime = txFileSystem.File.GetLastWriteTime(fileName);
            var modifiedWriteTime = DateTime.Now.AddDays(-7);

            txFileSystem.File.SetLastWriteTime(fileName, modifiedWriteTime);

            Assert.NotEqual(originalWriteTime, txFileSystem.File.GetLastWriteTime(fileName));
            Assert.Equal(modifiedWriteTime, txFileSystem.File.GetLastWriteTime(fileName));
        }

        [Fact]
#if SUPPRESS_SIMPLE_USING
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void SetLastWriteTimeOperation_ExceptionThrown_ResultsTimeNotChanged()
        {
            var fileName = "/tmp/filetochangewritetimeof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);
            var originalWriteTime = txFileSystem.File.GetLastWriteTime(fileName);
            var modifiedWriteTime = DateTime.Now.AddDays(-7);

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);

                    txFileSystem.File.SetLastWriteTime(fileName, modifiedWriteTime);

                    throw new Exception("Exception occurred right after modifying write time");
                }
            });

            Assert.Equal(originalWriteTime, txFileSystem.File.GetLastWriteTime(fileName));
            Assert.NotEqual(modifiedWriteTime, txFileSystem.File.GetLastWriteTime(fileName));
        }

        [Fact, FsFact]
        public void SetLastWriteTimeOperation_AsUtc_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetochangewritetimeof.txt";
            var dateTime = DateTime.UtcNow;

            fileSystemMock.Setup(f => f.File.SetLastWriteTimeUtc(It.Is<string>((s) => s == fileName),
                It.Is<DateTime>((d) => d == dateTime)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.SetLastWriteTimeUtc(fileName, dateTime);

            fileSystemMock.Verify(f => f.File.SetLastWriteTimeUtc(It.Is<string>((s) => s == fileName),
                It.Is<DateTime>((d) => d == dateTime)), Times.Once);
        }
    }
}
