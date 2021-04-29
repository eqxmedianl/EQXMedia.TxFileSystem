namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.TestingHelpers.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using Xunit;

    public sealed class GetLastAccessTimeOperation_Should
    {
        [Fact, FsFact]
        public void GetLastAccessTimeOperation_AsUtc_CalledOnce_ReturnsDateTime()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var dirName = "/tmp/dirtogetlastwritetimeof";
            var dateTime = DateTime.UtcNow;

            fileSystemMock.Setup(f => f.Directory.GetLastAccessTimeUtc(It.Is<string>((s) => s == dirName)))
                .Returns(dateTime);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var dateTimeReturned = txFileSystem.Directory.GetLastAccessTimeUtc(dirName);

            fileSystemMock.Verify(f => f.Directory.GetLastAccessTimeUtc(It.Is<string>((s) => s == dirName)), Times.Once);

            Assert.IsType<DateTime>(dateTimeReturned);
            Assert.Equal(dateTime, dateTimeReturned);
        }
    }
}
