namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.TestingHelpers.Attributes;
    using Moq;
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.Reflection;
    using Xunit;

    public sealed class GetAttributesOperation_Should
    {
        [Fact, FsFact]
        public void GetAttributesOperation_CalledOnce_ReturnsFileAttributes()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetogetattributesof.txt";
            var fileAttributes = new FileAttributes();

            fileSystemMock.Setup(f => f.File.GetAttributes(It.Is<string>((s) => s == fileName)))
                .Returns(fileAttributes);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var fileAttributesReturned = txFileSystem.File.GetAttributes(fileName);

            fileSystemMock.Verify(f => f.File.GetAttributes(It.Is<string>((s) => s == fileName)), Times.Once);

            Assert.IsType<FileAttributes>(fileAttributesReturned);
        }
    }
}
