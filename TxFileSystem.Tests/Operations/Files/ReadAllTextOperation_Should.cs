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

    public sealed class ReadAllTextOperation_Should
    {
        [Fact, FsFact]
        public void ReadAllTextOperation_Encoding_CalledOnce_ReturnsSameText()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/textfile.txt";
            var lines = new string[] { "Line one", "Line two", "Line three" };
            var text = string.Join(Environment.NewLine, lines);

            fileSystemMock.Setup(f => f.File.ReadAllText(It.Is<string>((s) => s == fileName),
                It.Is<Encoding>((e) => e == Encoding.UTF8)))
                .Returns(text);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var textReturned = txFileSystem.File.ReadAllText(fileName, Encoding.UTF8);

            fileSystemMock.Verify(f => f.File.ReadAllText(It.Is<string>((s) => s == fileName),
                It.Is<Encoding>((e) => e == Encoding.UTF8)), Times.Once);

            Assert.IsAssignableFrom<string>(textReturned);
            Assert.Equal(text, textReturned);
        }

#if ASYNC_IO
        [Fact, FsFact]
        public void ReadAllTextOperationAsync_CalledOnce_ReturnsSameStringTask()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/textfile.txt";
            var lines = new string[] { "Line one", "Line two", "Line three" };
            var text = string.Join(Environment.NewLine, lines);
            var task = Task.FromResult(text);

            fileSystemMock.Setup(f => f.File.ReadAllTextAsync(It.Is<string>((s) => s == fileName),
                It.IsAny<CancellationToken>()))
                .Returns(task);

            var cancellationTokenSource = new CancellationTokenSource();

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var taskReturned = txFileSystem.File.ReadAllTextAsync(fileName, cancellationTokenSource.Token);

            fileSystemMock.Verify(f => f.File.ReadAllTextAsync(It.Is<string>((s) => s == fileName),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.IsAssignableFrom<Task<string>>(taskReturned);
            Assert.Equal(task, taskReturned);
        }

        [Fact, FsFact]
        public void ReadAllLinesOperationAsync_Encoding_CalledOnce_ReturnsSameStringTask()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/textfile.txt";
            var lines = new string[] { "Line one", "Line two", "Line three" };
            var text = string.Join(Environment.NewLine, lines);
            var task = Task.FromResult(text);

            fileSystemMock.Setup(f => f.File.ReadAllTextAsync(It.Is<string>((s) => s == fileName),
                It.Is<Encoding>((e) => e == Encoding.UTF8), It.IsAny<CancellationToken>()))
                .Returns(task);

            var cancellationTokenSource = new CancellationTokenSource();

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var taskReturned = txFileSystem.File.ReadAllTextAsync(fileName, Encoding.UTF8, cancellationTokenSource.Token);

            fileSystemMock.Verify(f => f.File.ReadAllTextAsync(It.Is<string>((s) => s == fileName),
                It.Is<Encoding>((e) => e == Encoding.UTF8), It.IsAny<CancellationToken>()), Times.Once);

            Assert.IsAssignableFrom<Task<string>>(taskReturned);
            Assert.Equal(task, taskReturned);
        }
#endif
    }
}

