namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using Xunit;

    public sealed class WriteAllLinesOperation_Should
    {
        [Fact, FsFact]
        public void WriteAllLinesOperation_ContentsArray_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.WriteAllLines(It.IsAny<string>(), It.IsAny<string[]>()));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllLines("/tmp/filetowritelinesinto.txt", new string[] { "line one", "line two" });

            fileSystemMock.Verify(f => f.File.WriteAllLines(It.IsAny<string>(), It.IsAny<string[]>()),
                Times.Once);
        }

        [Fact, FsFact]
        public void WriteAllLinesOperation_ContentsEnumerable_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.WriteAllLines(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllLines("/tmp/filetowritelinesinto.txt", new string[] { "line one", "line two" }
                .AsEnumerable());

            fileSystemMock.Verify(f => f.File.WriteAllLines(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()),
                Times.Once);
        }

        [Fact, FsFact]
        public void WriteAllLinesOperation_ContentsArray_WithEncoding_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.WriteAllLines(It.IsAny<string>(), It.IsAny<string[]>(),
                It.IsAny<Encoding>()));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllLines("/tmp/filetowritelinesinto.txt", new string[] { "line one", "line two" },
                Encoding.ASCII);

            fileSystemMock.Verify(f => f.File.WriteAllLines(It.IsAny<string>(), It.IsAny<string[]>(),
                It.IsAny<Encoding>()), Times.Once);
        }

        [Fact, FsFact]
        public void WriteAllLinesOperation_ContentsEnumerable_WithEncoding_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.WriteAllLines(It.IsAny<string>(), It.IsAny<string[]>(),
                It.IsAny<Encoding>()));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllLines("/tmp/filetowritelinesinto.txt", new string[] { "line one", "line two" }
                .AsEnumerable(), Encoding.ASCII);

            fileSystemMock.Verify(f => f.File.WriteAllLines(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(),
                It.IsAny<Encoding>()), Times.Once);
        }

#if NETCOREAPP3_1_OR_GREATER
        [Fact, FsFact]
        public void WriteAllLinesOperationAsync_ContentsArray_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.WriteAllLinesAsync(It.IsAny<string>(), It.IsAny<string[]>(),
                It.IsAny<CancellationToken>()));

            var cancellationTokenSource = new CancellationTokenSource();

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllLinesAsync("/tmp/filetowritelinesinto.txt", new string[] { "line one", "line two" },
                cancellationTokenSource.Token);

            fileSystemMock.Verify(f => f.File.WriteAllLinesAsync(It.IsAny<string>(), It.IsAny<string[]>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact, FsFact]
        public void WriteAllLinesOperationAsync_ContentsEnumerable_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.WriteAllLinesAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(),
                It.IsAny<CancellationToken>()));

            var cancellationTokenSource = new CancellationTokenSource();

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllLinesAsync("/tmp/filetowritelinesinto.txt", new string[] { "line one", "line two" }
                .AsEnumerable(), cancellationTokenSource.Token);

            fileSystemMock.Verify(f => f.File.WriteAllLinesAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact, FsFact]
        public void WriteAllLinesOperationAsync_ContentsArray_WithEncoding_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.WriteAllLinesAsync(It.IsAny<string>(), It.IsAny<string[]>(),
                It.IsAny<Encoding>(), It.IsAny<CancellationToken>()));

            var cancellationTokenSource = new CancellationTokenSource();

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllLinesAsync("/tmp/filetowritelinesinto.txt", new string[] { "line one", "line two" },
                Encoding.ASCII, cancellationTokenSource.Token);

            fileSystemMock.Verify(f => f.File.WriteAllLinesAsync(It.IsAny<string>(), It.IsAny<string[]>(),
                It.IsAny<Encoding>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact, FsFact]
        public void WriteAllLinesOperationAsync_ContentsEnumerable_WithEncoding_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.WriteAllLinesAsync(It.IsAny<string>(), It.IsAny<string[]>(),
                It.IsAny<Encoding>(), It.IsAny<CancellationToken>()));

            var cancellationTokenSource = new CancellationTokenSource();

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.WriteAllLinesAsync("/tmp/filetowritelinesinto.txt", new string[] { "line one", "line two" }
                .AsEnumerable(), Encoding.ASCII, cancellationTokenSource.Token);

            fileSystemMock.Verify(f => f.File.WriteAllLinesAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(),
                It.IsAny<Encoding>(), It.IsAny<CancellationToken>()), Times.Once);
        }
#endif
    }
}
