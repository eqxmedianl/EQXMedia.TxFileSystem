namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Security.AccessControl;
    using Xunit;

    public sealed class GetAccessControlOperation_Should
    {
        [Fact, FsFact]
        public void GetAccessControlOperation_CalledOnce_ReturnsSameDirectorySecurity()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var dirName = "/tmp/dirtogetaccesscontrolof";
            var accessControl = new DirectorySecurity();

            fileSystemMock.Setup(f => f.Directory.GetAccessControl(It.Is<string>((s) => s == dirName)))
                .Returns(accessControl);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var accessControlReturned = txFileSystem.Directory.GetAccessControl(dirName);

            fileSystemMock.Verify(f => f.Directory.GetAccessControl(It.Is<string>((s) => s == dirName)), Times.Once);

            Assert.IsType<DirectorySecurity>(accessControlReturned);
            Assert.Equal(accessControl, accessControlReturned);
        }

        [Fact, FsFact]
        public void GetAccessControlOperation_AccassControlSections_CalledOnce_ReturnsSameDirectorySecurity()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var dirName = "/tmp/dirtogetaccesscontrolof";
            var accessControl = new DirectorySecurity();
            var includeSections = AccessControlSections.Owner;

            fileSystemMock.Setup(f => f.Directory.GetAccessControl(It.Is<string>((s) => s == dirName),
                It.Is<AccessControlSections>((s) => s == includeSections)))
                .Returns(accessControl);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var accessControlReturned = txFileSystem.Directory.GetAccessControl(dirName, includeSections);

            fileSystemMock.Verify(f => f.Directory.GetAccessControl(It.Is<string>((s) => s == dirName),
                It.Is<AccessControlSections>((s) => s == includeSections)), Times.Once);

            Assert.IsType<DirectorySecurity>(accessControlReturned);
            Assert.Equal(accessControl, accessControlReturned);
        }
    }
}
