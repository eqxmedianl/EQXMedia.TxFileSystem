namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using Xunit;

    public sealed class WriteAllTextOperation_Should
    {
        [Fact, FsFact]
        public void WriteAllTextOperation_ContentsOnly_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/textfile.txt";
            var contents = "This is the contents for the file";

            fileSystemMock.Setup(f => f.File.WriteAllText(It.Is<string>((s) => s == fileName),
                It.Is<string>((s) => s == contents)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllText(fileName, contents);

            fileSystemMock.Verify(f => f.File.WriteAllText(It.Is<string>((s) => s == fileName),
                It.Is<string>((s) => s == contents)), Times.Once);
        }

        [Fact, FsFact]
        public void WriteAllTextOperation_EncodedContents_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/textfile.txt";
            var contents = "This is the contents for the file";

            fileSystemMock.Setup(f => f.File.WriteAllText(It.Is<string>((s) => s == fileName),
                It.Is<string>((s) => s == contents), It.Is<Encoding>((e) => e == Encoding.ASCII)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllText(fileName, contents, Encoding.ASCII);

            fileSystemMock.Verify(f => f.File.WriteAllText(It.Is<string>((s) => s == fileName),
                It.Is<string>((s) => s == contents), It.Is<Encoding>((e) => e == Encoding.ASCII)), Times.Once);
        }

#if ASYNC_IO
        [Fact, FsFact]
        public void WriteAllTextOperationAsync_ContentsOnly_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/textfile.txt";
            var contents = "This is the contents for the file";

            fileSystemMock.Setup(f => f.File.WriteAllTextAsync(It.Is<string>((s) => s == fileName),
                It.Is<string>((s) => s == contents), It.IsAny<CancellationToken>()));

            var cancellationTokenSource = new CancellationTokenSource();

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllTextAsync(fileName, contents, cancellationTokenSource.Token);

            fileSystemMock.Verify(f => f.File.WriteAllTextAsync(It.Is<string>((s) => s == fileName),
                It.Is<string>((s) => s == contents), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact, FsFact]
        public void WriteAllTextOperationAsync_EncodedContents_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/textfile.txt";
            var contents = "This is the contents for the file";

            fileSystemMock.Setup(f => f.File.WriteAllTextAsync(It.Is<string>((s) => s == fileName),
                It.Is<string>((s) => s == contents), It.Is<Encoding>((e) => e == Encoding.ASCII),
                It.IsAny<CancellationToken>()));

            var cancellationTokenSource = new CancellationTokenSource();

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllTextAsync(fileName, contents, Encoding.ASCII, cancellationTokenSource.Token);

            fileSystemMock.Verify(f => f.File.WriteAllTextAsync(It.Is<string>((s) => s == fileName),
                It.Is<string>((s) => s == contents), It.Is<Encoding>((e) => e == Encoding.ASCII),
                It.IsAny<CancellationToken>()), Times.Once);
        }
#endif
    }
}
