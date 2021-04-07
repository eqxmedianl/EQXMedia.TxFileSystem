namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using Xunit;

    public sealed class GetLastWriteTimeOperation_Should
    {
        [Fact, FsFact]
        public void GetLastWriteTimeOperation_AsUtc_CalledOnce_ReturnsDateTime()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetogetlastwritetimeof.txt";
            var dateTime = DateTime.Now;

            fileSystemMock.Setup(f => f.File.GetLastWriteTimeUtc(It.Is<string>((s) => s == fileName)))
                .Returns(dateTime);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var dateTimeReturned = txFileSystem.File.GetLastWriteTimeUtc(fileName);

            fileSystemMock.Verify(f => f.File.GetLastWriteTimeUtc(It.Is<string>((s) => s == fileName)), Times.Once);

            Assert.IsType<DateTime>(dateTimeReturned);
            Assert.Equal(dateTime, dateTimeReturned);
        }
    }
}
