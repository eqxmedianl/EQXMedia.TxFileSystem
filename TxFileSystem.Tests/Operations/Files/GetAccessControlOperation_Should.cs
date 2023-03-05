namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.TestingHelpers.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Security.AccessControl;
    using Xunit;

#if SUPPORTED_OS_PLATFORM
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
#endif
    public sealed class GetAccessControlOperation_Should
    {
        [Fact, FsFact]
        public void GetAccessControlOperation_CalledOnce_ReturnsFileSecurity()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetogetaccesscontrolof.txt";
            var fileSecurity = new FileSecurity();

            fileSystemMock.Setup(f => f.File.GetAccessControl(It.Is<string>((s) => s == fileName)))
                .Returns(fileSecurity);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var fileSecurityReturned = txFileSystem.File.GetAccessControl(fileName);

            fileSystemMock.Verify(f => f.File.GetAccessControl(It.Is<string>((s) => s == fileName)), Times.Once);

            Assert.IsType<FileSecurity>(fileSecurityReturned);
        }

        [Fact, FsFact]
        public void GetAccessControlOperation_WithAccessControlSection_CalledOnce_ReturnsFileSecurity()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetogetaccesscontrolof.txt";
            var fileSecurity = new FileSecurity();

            fileSystemMock.Setup(f => f.File.GetAccessControl(It.Is<string>((s) => s == fileName),
                It.Is<AccessControlSections>((s) => s == AccessControlSections.None)))
                .Returns(fileSecurity);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var fileSecurityReturned = txFileSystem.File.GetAccessControl(fileName, AccessControlSections.None);

            fileSystemMock.Verify(f => f.File.GetAccessControl(It.Is<string>((s) => s == fileName),
                It.Is<AccessControlSections>((s) => s == AccessControlSections.None)), Times.Once);

            Assert.IsType<FileSecurity>(fileSecurityReturned);
        }
    }
}
