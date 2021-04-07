namespace EQXMedia.TxFileSystem.Tests
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Reflection;
    using Xunit;

    public sealed class TxPath_Should
    {
        [Fact]
        public void TxPath_FileSystem_SameFileSystemReturned()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            Assert.Equal(mockFileSystem, ((ITxPath)txFileSystem.Path).FileSystem);
        }

        [Fact]
        public void TxPath_AltDirectorySeparatorChar_SameAltDirectorySeparatorCharReturned()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            Assert.Equal(mockFileSystem.Path.AltDirectorySeparatorChar, txFileSystem.Path.AltDirectorySeparatorChar);
        }

        [Fact]
        public void TxPath_DirectorySeparatorChar_SameDirectorySeparatorCharReturned()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            Assert.Equal(mockFileSystem.Path.DirectorySeparatorChar, txFileSystem.Path.DirectorySeparatorChar);
        }

        [Fact]
        public void TxPath_PathSeparator_SamePathSeparatorReturned()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            Assert.Equal(mockFileSystem.Path.PathSeparator, txFileSystem.Path.PathSeparator);
        }

        [Fact]
        public void TxPath_VolumeSeparatorChar_SameVolumeSeparatorCharReturned()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            Assert.Equal(mockFileSystem.Path.VolumeSeparatorChar, txFileSystem.Path.VolumeSeparatorChar);
        }

        [Fact]
        public void TxPath_InvalidPathChars_SameInvalidPathCharsReturned()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            Assert.Equal(mockFileSystem.Path.InvalidPathChars, txFileSystem.Path.InvalidPathChars);
        }

        [Fact, FsFact]
        public void TxPath_ChangeExtension_CalledOnce_ReturnsChangedPath()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path = "/tmp/subjectpath.info";
            var çhangedPath = "/tmp/subjectpath.txt";
            var extension = "txt";

            fileSystemMock.Setup(f => f.Path.ChangeExtension(It.Is<string>((p) => p == path),
                It.Is<string>((e) => e == extension)))
                .Returns(çhangedPath);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var pathReturned = txFileSystem.Path.ChangeExtension(path, extension);

            fileSystemMock.Verify(f => f.Path.ChangeExtension(It.Is<string>((p) => p == path),
                It.Is<string>((e) => e == extension)), Times.Once);

            Assert.Equal(çhangedPath, pathReturned);
        }

        [Fact, FsFact]
        public void TxPath_Combine_TwoPaths_CalledOnce_ReturnsChangedPath()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path1 = "/tmp/subjectpath1.info";
            var path2 = "/tmp/subjectpath2.info";
            var çhangedPath = "/tmp/unknownresultpath.txt";

            fileSystemMock.Setup(f => f.Path.Combine(It.Is<string>((p) => p == path1), It.Is<string>((p) => p == path2)))
                .Returns(çhangedPath);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var pathReturned = txFileSystem.Path.Combine(path1, path2);

            fileSystemMock.Verify(f => f.Path.Combine(It.Is<string>((p) => p == path1),
                It.Is<string>((p) => p == path2)), Times.Once);

            Assert.Equal(çhangedPath, pathReturned);
        }

        [Fact, FsFact]
        public void TxPath_Combine_ThreePaths_CalledOnce_ReturnsChangedPath()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path1 = "/tmp/subjectpath1.info";
            var path2 = "/tmp/subjectpath2.info";
            var path3 = "/tmp/subjectpath3.info";
            var çhangedPath = "/tmp/unknownresultpath.txt";

            fileSystemMock.Setup(f => f.Path.Combine(It.Is<string>((p) => p == path1), It.Is<string>((p) => p == path2),
                It.Is<string>((p) => p == path3)))
                .Returns(çhangedPath);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var pathReturned = txFileSystem.Path.Combine(path1, path2, path3);

            fileSystemMock.Verify(f => f.Path.Combine(It.Is<string>((p) => p == path1),
                It.Is<string>((p) => p == path2), It.Is<string>((p) => p == path3)), Times.Once);

            Assert.Equal(çhangedPath, pathReturned);
        }

        [Fact, FsFact]
        public void TxPath_Combine_FourPaths_CalledOnce_ReturnsChangedPath()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path1 = "/tmp/subjectpath1.info";
            var path2 = "/tmp/subjectpath2.info";
            var path3 = "/tmp/subjectpath3.info";
            var path4 = "/tmp/subjectpath4.info";
            var çhangedPath = "/tmp/unknownresultpath.txt";

            fileSystemMock.Setup(f => f.Path.Combine(It.Is<string>((p) => p == path1), It.Is<string>((p) => p == path2),
                It.Is<string>((p) => p == path3), It.Is<string>((p) => p == path4)))
                .Returns(çhangedPath);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var pathReturned = txFileSystem.Path.Combine(path1, path2, path3, path4);

            fileSystemMock.Verify(f => f.Path.Combine(It.Is<string>((p) => p == path1),
                It.Is<string>((p) => p == path2), It.Is<string>((p) => p == path3),
                It.Is<string>((p) => p == path4)), Times.Once);

            Assert.Equal(çhangedPath, pathReturned);
        }

        [Fact, FsFact]
        public void TxPath_Combine_ParamsPaths_CalledOnce_ReturnsChangedPath()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var paths = new string[]
            {
                "/tmp/subjectpath1.info",
                "/tmp/subjectpath2.info",
                "/tmp/subjectpath3.info",
                "/tmp/subjectpath4.info"
            };
            var çhangedPath = "/tmp/unknownresultpath.txt";

            fileSystemMock.Setup(f => f.Path.Combine(It.Is<string[]>((p) => p == paths)))
                .Returns(çhangedPath);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var pathReturned = txFileSystem.Path.Combine(paths);

            fileSystemMock.Verify(f => f.Path.Combine(It.Is<string[]>((p) => p == paths)), Times.Once);

            Assert.Equal(çhangedPath, pathReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetDirectoryName_CalledOnce_ReturnsDirectoryName()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path = "/tmp/subjectpath1.info";
            var directoryName = "/tmp";

            fileSystemMock.Setup(f => f.Path.GetDirectoryName(It.Is<string>((p) => p == path)))
                .Returns(directoryName);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var directoryNameReturned = txFileSystem.Path.GetDirectoryName(path);

            fileSystemMock.Verify(f => f.Path.GetDirectoryName(It.Is<string>((p) => p == path)), Times.Once);

            Assert.Equal(directoryName, directoryNameReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetExtension_CalledOnce_ReturnsExtension()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path = "/tmp/subjectpath1.info";
            var extension = ".info";

            fileSystemMock.Setup(f => f.Path.GetExtension(It.Is<string>((p) => p == path)))
                .Returns(extension);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var extensionReturned = txFileSystem.Path.GetExtension(path);

            fileSystemMock.Verify(f => f.Path.GetExtension(It.Is<string>((p) => p == path)), Times.Once);

            Assert.Equal(extension, extensionReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetFileName_CalledOnce_ReturnsFileName()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path = "/tmp/subjectpath1.info";
            var fileName = "subjectpath1.info";

            fileSystemMock.Setup(f => f.Path.GetFileName(It.Is<string>((p) => p == path)))
                .Returns(fileName);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fileNameReturned = txFileSystem.Path.GetFileName(path);

            fileSystemMock.Verify(f => f.Path.GetFileName(It.Is<string>((p) => p == path)), Times.Once);

            Assert.Equal(fileName, fileNameReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetFileNameWithoutExtension_CalledOnce_ReturnsFileNameWithoutExtension()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path = "/tmp/subjectpath1.info";
            var fileNameWithoutExtension = "subjectpath1";

            fileSystemMock.Setup(f => f.Path.GetFileNameWithoutExtension(It.Is<string>((p) => p == path)))
                .Returns(fileNameWithoutExtension);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fileNameWithoutExtensionReturned = txFileSystem.Path.GetFileNameWithoutExtension(path);

            fileSystemMock.Verify(f => f.Path.GetFileNameWithoutExtension(It.Is<string>((p) => p == path)),
                Times.Once);

            Assert.Equal(fileNameWithoutExtension, fileNameWithoutExtensionReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetFullPath_CalledOnce_ReturnsFullPath()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path = "subjectdirectory";
            var fullPath = "/tmp/currentdirectory";

            fileSystemMock.Setup(f => f.Path.GetFullPath(It.Is<string>((p) => p == path)))
                .Returns(fullPath);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fullPathReturned = txFileSystem.Path.GetFullPath(path);

            fileSystemMock.Verify(f => f.Path.GetFullPath(It.Is<string>((p) => p == path)),
                Times.Once);

            Assert.Equal(fullPath, fullPathReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetFullPath_WithBasePath_CalledOnce_ReturnsFullPath()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path = "subjectdirectory";
            var basePath = "/tmp/basedirectory";
            var fullPath = "/tmp/basedirectory/subjectdirectory";

            fileSystemMock.Setup(f => f.Path.GetFullPath(It.Is<string>((p) => p == path),
                It.Is<string>((p) => p == basePath)))
                .Returns(fullPath);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fullPathReturned = txFileSystem.Path.GetFullPath(path, basePath);

            fileSystemMock.Verify(f => f.Path.GetFullPath(It.Is<string>((p) => p == path),
                It.Is<string>((p) => p == basePath)), Times.Once);

            Assert.Equal(fullPath, fullPathReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetPathRoot_CalledOnce_ReturnsPathRoot()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path = @"\mydir\";
            var pathRoot = @"\";

            fileSystemMock.Setup(f => f.Path.GetPathRoot(It.Is<string>((p) => p == path)))
                .Returns(pathRoot);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var pathRootReturned = txFileSystem.Path.GetPathRoot(path);

            fileSystemMock.Verify(f => f.Path.GetPathRoot(It.Is<string>((p) => p == path)), Times.Once);

            Assert.Equal(pathRoot, pathRootReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetRandomFileName_CalledOnce_ReturnsRandomFileName()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var randomFileName = "JHDKu8i2087";

            fileSystemMock.Setup(f => f.Path.GetRandomFileName())
                .Returns(randomFileName);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var randomFileNameReturned = txFileSystem.Path.GetRandomFileName();

            fileSystemMock.Verify(f => f.Path.GetRandomFileName(), Times.Once);

            Assert.Equal(randomFileName, randomFileNameReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetRelativePath_CalledOnce_ReturnsRelativePath()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var relativeTo = "/tmp/parentdir";
            var path = "/tmp/parentdir/subdir/thefile.txt";
            var relativePath = "subdir/thefile.txt";

            fileSystemMock.Setup(f => f.Path.GetRelativePath(It.Is<string>((e) => e == relativeTo),
                It.Is<string>((e) => e == path)))
                .Returns(relativePath);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var relativePathReturned = txFileSystem.Path.GetRelativePath(relativeTo, path);

            fileSystemMock.Verify(f => f.Path.GetRelativePath(It.Is<string>((e) => e == relativeTo),
                It.Is<string>((e) => e == path)), Times.Once);

            Assert.Equal(relativePath, relativePathReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetTempFileName_CalledOnce_ReturnsTempFileName()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var tempFileName = "723198fho";

            fileSystemMock.Setup(f => f.Path.GetTempFileName())
                .Returns(tempFileName);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var tempFileNameReturned = txFileSystem.Path.GetTempFileName();

            fileSystemMock.Verify(f => f.Path.GetTempFileName(), Times.Once);

            Assert.Equal(tempFileName, tempFileNameReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetTempPath_CalledOnce_ReturnsTempPath()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var tempPath = "/tmp";

            fileSystemMock.Setup(f => f.Path.GetTempPath())
                .Returns(tempPath);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var tempPathReturned = txFileSystem.Path.GetTempPath();

            fileSystemMock.Verify(f => f.Path.GetTempPath(), Times.Once);

            Assert.Equal(tempPath, tempPathReturned);
        }

        [Fact, FsFact]
        public void TxPath_HasExtention_CalledOnce_ReturnsHasExtention()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filename.ext";
            var hasExtension = true;

            fileSystemMock.Setup(f => f.Path.HasExtension(It.Is<string>((e) => e == fileName)))
                .Returns(hasExtension);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var hasExtensionReturned = txFileSystem.Path.HasExtension(fileName);

            fileSystemMock.Verify(f => f.Path.HasExtension(It.Is<string>((e) => e == fileName)), Times.Once);

            Assert.Equal(hasExtension, hasExtensionReturned);
        }

        [Fact, FsFact]
        public void TxPath_IsPathFullyQualified_CalledOnce_ReturnsIsPathFullyQualified()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var path = "/tmp/filename.ext";
            var isPathFullyQualified = true;

            fileSystemMock.Setup(f => f.Path.IsPathFullyQualified(It.Is<string>((e) => e == path)))
                .Returns(isPathFullyQualified);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var isPathFullyQualifiedReturned = txFileSystem.Path.IsPathFullyQualified(path);

            fileSystemMock.Verify(f => f.Path.IsPathFullyQualified(It.Is<string>((e) => e == path)), Times.Once);

            Assert.Equal(isPathFullyQualified, isPathFullyQualifiedReturned);
        }

        [Fact, FsFact]
        public void TxPath_IsPathRooted_CalledOnce_ReturnsIsPathRooted()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filename.ext";
            var isPathRooted = true;

            fileSystemMock.Setup(f => f.Path.IsPathRooted(It.Is<string>((e) => e == fileName)))
                .Returns(isPathRooted);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var isPathRootedReturned = txFileSystem.Path.IsPathRooted(fileName);

            fileSystemMock.Verify(f => f.Path.IsPathRooted(It.Is<string>((e) => e == fileName)), Times.Once);

            Assert.Equal(isPathRooted, isPathRootedReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetInvalidFileNameChars_CalledOnce_ReturnsInvalidFileNameChars()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var invalidFileNameChars = new char[] { ';', '/' };

            fileSystemMock.Setup(f => f.Path.GetInvalidFileNameChars())
                .Returns(invalidFileNameChars);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var invalidFileNameCharsReturned = txFileSystem.Path.GetInvalidFileNameChars();

            fileSystemMock.Verify(f => f.Path.GetInvalidFileNameChars(), Times.Once);

            Assert.Equal(invalidFileNameChars, invalidFileNameCharsReturned);
        }

        [Fact, FsFact]
        public void TxPath_GetInvalidPathChars_CalledOnce_ReturnsInvalidPathChars()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var invalidPathChars = new char[] { ';', '/' };

            fileSystemMock.Setup(f => f.Path.GetInvalidPathChars())
                .Returns(invalidPathChars);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var invalidPathCharsReturned = txFileSystem.Path.GetInvalidPathChars();

            fileSystemMock.Verify(f => f.Path.GetInvalidPathChars(), Times.Once);

            Assert.Equal(invalidPathChars, invalidPathCharsReturned);
        }

        [Fact]
        public void TxPath_Join_TwoPaths_CalledOnce_ReturnsJoinedPaths()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            var path1 = new ReadOnlySpan<char>("/tmp/".ToCharArray());
            var path2 = new ReadOnlySpan<char>("subdir".ToCharArray());

            Assert.Equal(mockFileSystem.Path.Join(path1, path2), txFileSystem.Path.Join(path1, path2));
        }

        [Fact]
        public void TxPath_Join_ThreePaths_CalledOnce_ReturnsJoinedPaths()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            var path1 = new ReadOnlySpan<char>("/tmp/".ToCharArray());
            var path2 = new ReadOnlySpan<char>("subdir/".ToCharArray());
            var path3 = new ReadOnlySpan<char>("subdirchild".ToCharArray());

            Assert.Equal(mockFileSystem.Path.Join(path1, path2, path3),
                txFileSystem.Path.Join(path1, path2, path3));
        }

        [Fact]
        public void TxPath_TryJoin_TwoPaths_WithDestinationAndCharsWritten_CalledOnce_ReturnsJoinedPaths()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            var path1 = new ReadOnlySpan<char>("/tmp/".ToCharArray());
            var path2 = new ReadOnlySpan<char>("subdir/".ToCharArray());

            var destinationOne = new Span<char>(new char[] { });
            var destinationTwo = new Span<char>(new char[] { });

            mockFileSystem.Path.TryJoin(path1, path2, destinationOne, out int charsWrittenOne);
            txFileSystem.Path.TryJoin(path1, path2, destinationTwo, out int charsWrittenTwo);

            Assert.Equal(destinationOne.ToString(), destinationTwo.ToString());
            Assert.Equal(charsWrittenOne, charsWrittenTwo);
        }

        [Fact]
        public void TxPath_TryJoin_ThreePaths_WithDestinationAndCharsWritten_CalledOnce_ReturnsJoinedPaths()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            var path1 = new ReadOnlySpan<char>("/tmp/".ToCharArray());
            var path2 = new ReadOnlySpan<char>("subdir/".ToCharArray());
            var path3 = new ReadOnlySpan<char>("subdirchild".ToCharArray());

            var destinationOne = new Span<char>(new char[] { });
            var destinationTwo = new Span<char>(new char[] { });

            mockFileSystem.Path.TryJoin(path1, path2, path3, destinationOne, out int charsWrittenOne);
            txFileSystem.Path.TryJoin(path1, path2, path3, destinationTwo, out int charsWrittenTwo);

            Assert.Equal(destinationOne.ToString(), destinationTwo.ToString());
            Assert.Equal(charsWrittenOne, charsWrittenTwo);
        }
    }
}
