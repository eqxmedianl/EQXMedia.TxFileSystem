namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.TestingHelpers.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class ReadAllLinesOperation_Should
    {
        [Fact, FsFact]
        public void ReadAllLinesOperation_Encoding_CalledOnce_ReturnsStringArray()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/textfile.txt";
            var lines = new string[] { "Line one", "Line two", "Line three" };

            fileSystemMock.Setup(f => f.File.ReadAllLines(It.Is<string>((s) => s == fileName),
                It.Is<Encoding>((e) => e == Encoding.UTF8)))
                .Returns(lines);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var linesReturned = txFileSystem.File.ReadAllLines(fileName, Encoding.UTF8);

            fileSystemMock.Verify(f => f.File.ReadAllLines(It.Is<string>((s) => s == fileName),
                It.Is<Encoding>((e) => e == Encoding.UTF8)), Times.Once);

            Assert.IsType<string[]>(linesReturned);
            Assert.NotEmpty(linesReturned);
            Assert.Equal(lines, linesReturned);
        }

#if ASYNC_IO
        [Fact, FsFact]
        public void ReadAllLinesOperationAsync_CalledOnce_ReturnsStringArrayTask()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/textfile.txt";
            var lines = new string[] { "Line one", "Line two", "Line three" };
            var task = Task.FromResult(lines);

            fileSystemMock.Setup(f => f.File.ReadAllLinesAsync(It.Is<string>((s) => s == fileName),
                It.IsAny<CancellationToken>()))
                .Returns(task);

            var cancellationTokenSource = new CancellationTokenSource();

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var taskReturned = txFileSystem.File.ReadAllLinesAsync(fileName, cancellationTokenSource.Token);

            fileSystemMock.Verify(f => f.File.ReadAllLinesAsync(It.Is<string>((s) => s == fileName),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.IsType<Task<string[]>>(taskReturned);
            Assert.NotEmpty(taskReturned.Result);
            Assert.Equal(task, taskReturned);
        }

        [Fact, FsFact]
        public void ReadAllLinesOperationAsync_Encoding_CalledOnce_ReturnsStringArrayTask()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/textfile.txt";
            var lines = new string[] { "Line one", "Line two", "Line three" };
            var task = Task.FromResult(lines);

            fileSystemMock.Setup(f => f.File.ReadAllLinesAsync(It.Is<string>((s) => s == fileName),
                It.Is<Encoding>((e) => e == Encoding.UTF8), It.IsAny<CancellationToken>()))
                .Returns(task);

            var cancellationTokenSource = new CancellationTokenSource();

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var taskReturned = txFileSystem.File.ReadAllLinesAsync(fileName, Encoding.UTF8, cancellationTokenSource.Token);

            fileSystemMock.Verify(f => f.File.ReadAllLinesAsync(It.Is<string>((s) => s == fileName),
                It.Is<Encoding>((e) => e == Encoding.UTF8), It.IsAny<CancellationToken>()), Times.Once);

            Assert.IsType<Task<string[]>>(taskReturned);
            Assert.NotEmpty(taskReturned.Result);
            Assert.Equal(task, taskReturned);
        }
#endif
    }
}
