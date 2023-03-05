namespace EQXMedia.TxFileSystem.Tests.Operations.Files
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

            var fileName = "/tmp/filetogetlastaccesstimeof.txt";
            var dateTime = DateTime.Now;

            fileSystemMock.Setup(f => f.File.GetLastAccessTimeUtc(It.Is<string>((s) => s == fileName)))
                .Returns(dateTime);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var dateTimeReturned = txFileSystem.File.GetLastAccessTimeUtc(fileName);

            fileSystemMock.Verify(f => f.File.GetLastAccessTimeUtc(It.Is<string>((s) => s == fileName)), Times.Once);

            Assert.IsType<DateTime>(dateTimeReturned);
            Assert.Equal(dateTime, dateTimeReturned);
        }
    }
}
