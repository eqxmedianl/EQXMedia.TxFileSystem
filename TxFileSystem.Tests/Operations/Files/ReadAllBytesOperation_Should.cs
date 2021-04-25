namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Threading;
    using Xunit;

    public sealed class ReadAllBytesOperation_Should
    {
#if ASYNC_IO
        [Fact, FsFact]
        public void ReadAllBytesOperationAsync_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/textfile.txt";

            fileSystemMock.Setup(f => f.File.ReadAllBytesAsync(It.Is<string>((s) => s == fileName),
                It.IsAny<CancellationToken>()));

            var cancellationTokenSource = new CancellationTokenSource();

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.ReadAllBytesAsync(fileName, cancellationTokenSource.Token);

            fileSystemMock.Verify(f => f.File.ReadAllBytesAsync(It.Is<string>((s) => s == fileName),
                It.IsAny<CancellationToken>()), Times.Once);
        }
#endif
    }
}
