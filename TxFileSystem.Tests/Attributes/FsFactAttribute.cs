namespace EQXMedia.TxFileSystem.Tests.Attributes
{
    using Moq;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public sealed class FsFactAttribute : MockingFactAttribute
    {
        public FsFactAttribute([CallerMemberName] string unitTestMethodName = "")
            : base(unitTestMethodName)
        {
        }

        public string FileName { get; set; }

        public override void Before(MethodInfo methodUnderTest)
        {
            base.Before(methodUnderTest);

            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.SetupGet(f => f.Path)
                .Returns(() =>
                {
                    var pathMock = new Mock<IPath>();
                    pathMock.SetupGet(p => p.DirectorySeparatorChar)
                        .Returns('/');

                    return pathMock.Object;
                });

            fileSystemMock.SetupGet(f => f.FileInfo)
                .Returns(() =>
                {
                    var fileInfoFactoryMock = new Mock<IFileInfoFactory>();
                    fileInfoFactoryMock.Setup(i => i.FromFileName(It.IsAny<string>()))
                        .Returns<string>((p) =>
                        {
                            var fileInfoMock = new Mock<IFileInfo>();
                            fileInfoMock.SetupGet(i => i.Name)
                                .Returns(p.Substring(p.LastIndexOf(@"/") + 1));

                            return fileInfoMock.Object;
                        });

                    return fileInfoFactoryMock.Object;
                });

            fileSystemMock.Setup(f => f.DirectoryInfo.FromDirectoryName(It.IsAny<string>()))
                .Returns<string>((p) =>
                {
                    var parentDirectoryInfoMock = new Mock<IDirectoryInfo>();
                    parentDirectoryInfoMock.SetupGet(d => d.FullName)
                        .Returns(p.Substring(0, p.LastIndexOf(@"/")));

                    var directoryInfoMock = new Mock<IDirectoryInfo>();
                    directoryInfoMock.SetupGet(d => d.Parent)
                        .Returns(parentDirectoryInfoMock.Object);

                    return directoryInfoMock.Object;
                });

            AddMock(fileSystemMock);
        }
    }
}
