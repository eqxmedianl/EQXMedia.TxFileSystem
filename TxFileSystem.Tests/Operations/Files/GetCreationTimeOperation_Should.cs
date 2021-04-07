namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using Xunit;

    public sealed class GetCreationTimeOperation_Should
    {
        [Fact, FsFact]
        public void GetCreationTimeOperation_CalledOnce_ReturnsDateTime()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetogetcreationtimeof.txt";
            var dateTime = DateTime.Now;

            fileSystemMock.Setup(f => f.File.GetCreationTime(It.Is<string>((s) => s == fileName)))
                .Returns(dateTime);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var dateTimeReturned = txFileSystem.File.GetCreationTime(fileName);

            fileSystemMock.Verify(f => f.File.GetCreationTime(It.Is<string>((s) => s == fileName)), Times.Once);

            Assert.IsType<DateTime>(dateTimeReturned);
            Assert.Equal(dateTime, dateTimeReturned);
        }

        [Fact, FsFact]
        public void GetCreationTimeOperation_AsUtc_CalledOnce_ReturnsDateTime()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetogetcreationtimeof.txt";
            var dateTime = DateTime.UtcNow;

            fileSystemMock.Setup(f => f.File.GetCreationTimeUtc(It.Is<string>((s) => s == fileName)))
                .Returns(dateTime);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var dateTimeReturned = txFileSystem.File.GetCreationTimeUtc(fileName);

            fileSystemMock.Verify(f => f.File.GetCreationTimeUtc(It.Is<string>((s) => s == fileName)), Times.Once);

            Assert.IsType<DateTime>(dateTimeReturned);
            Assert.Equal(dateTime, dateTimeReturned);
        }
    }
}
