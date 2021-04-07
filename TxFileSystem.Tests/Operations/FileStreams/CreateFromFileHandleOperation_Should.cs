namespace EQXMedia.TxFileSystem.Tests.Operations.FileStreams
{
    using global::EQXMedia.TxFileSystem.NativeMethods.Win32;
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using global::EQXMedia.TxFileSystem.Tests.Operations.FileStreams.Utils;
    using Moq;
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Text;
    using System.Transactions;
    using Xunit;
    using FileAttributes = global::EQXMedia.TxFileSystem.NativeMethods.Win32.FileAttributes;

    public sealed class CreateFromFileHandleOperation_Should
    {
        [Fact]
        [Obsolete("Eventhough FileStream.Create(IntPtr handle, FileAccess access) is deprecated it is still part of the interface")]
        public void CreateFromFileHandleOperation_ContentsChanged()
        {
            var fileSystem = new FileSystem();
            var txFileSystem = new TxFileSystem(fileSystem);

            string fileName = UnitTestUtils.GetTempFileName(txFileSystem);

            var streamWriter = txFileSystem.File.CreateText(fileName);
            streamWriter.Write("Original contents" + Environment.NewLine);
            streamWriter.Flush();
            streamWriter.Close();

            var filePtr = NativeMethods.CreateFile(fileName, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero,
                FileMode.OpenOrCreate, FileAttributes.Normal, IntPtr.Zero);

            using var transactionScope = new TransactionScope();

            txFileSystem = new TxFileSystem(fileSystem);
            var fileStream = txFileSystem.FileStream.Create(filePtr, FileAccess.ReadWrite);
            var data = Encoding.Default.GetBytes("Written" + Environment.NewLine);
            fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
            fileStream.SetLength(fileStream.Length + data.Length);
            fileStream.Write(data, 0, data.Length);
            fileStream.Flush();

            transactionScope.Complete();

            NativeMethods.CloseHandle(filePtr);
            fileStream.Close();

            Assert.Equal("Original contents" + Environment.NewLine
                + "Written" + Environment.NewLine, txFileSystem.File.ReadAllText(fileName));

            txFileSystem.File.Delete(fileName);
        }

        [Fact]
        [Obsolete("Eventhough FileStream.Create(IntPtr handle, FileAccess access) is deprecated it is still part of the interface")]
        public void CreateFromFileHandleOperation_ExceptionThrown_ContentsUnchanged()
        {
            var fileSystem = new FileSystem();
            var txFileSystem = new TxFileSystem(fileSystem);

            string fileName = UnitTestUtils.GetTempFileName(txFileSystem);

            var streamWriter = txFileSystem.File.CreateText(fileName);
            streamWriter.Write("Original contents" + Environment.NewLine);
            streamWriter.Flush();
            streamWriter.Close();

            var filePtr = NativeMethods.CreateFile(fileName, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero,
                FileMode.OpenOrCreate, FileAttributes.Normal, IntPtr.Zero);

            Stream fileStream = null;
            var data = Encoding.Default.GetBytes("Written" + Environment.NewLine);

            Assert.ThrowsAny<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();

                txFileSystem = new TxFileSystem(fileSystem);
                fileStream = txFileSystem.FileStream.Create(filePtr, FileAccess.ReadWrite);
                fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
                fileStream.SetLength(fileStream.Length + data.Length);
                fileStream.Write(data, 0, data.Length);
                fileStream.Flush();

                throw new Exception("Error occurred right after writing to stream");
            });

            NativeMethods.CloseHandle(filePtr);
            fileStream.Close();

            Assert.Equal("Original contents" + Environment.NewLine, txFileSystem.File.ReadAllText(fileName));

            txFileSystem.File.Delete(fileName);
        }

        [Fact]
        [Obsolete("Eventhough FileStream.Create(IntPtr handle, FileAccess access, bool ownsHandle) is deprecated it is still part of the interface")]
        public void CreateFromFileHandleOperation_OwnsHandle_ContentsChanged()
        {
            var fileSystem = new FileSystem();
            var txFileSystem = new TxFileSystem(fileSystem);

            string fileName = UnitTestUtils.GetTempFileName(txFileSystem);

            var streamWriter = txFileSystem.File.CreateText(fileName);
            streamWriter.Write("Original contents" + Environment.NewLine);
            streamWriter.Flush();
            streamWriter.Close();

            var filePtr = NativeMethods.CreateFile(fileName, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero,
                FileMode.OpenOrCreate, FileAttributes.Normal, IntPtr.Zero);

            using var transactionScope = new TransactionScope();

            txFileSystem = new TxFileSystem(fileSystem);
            var fileStream = txFileSystem.FileStream.Create(filePtr, FileAccess.ReadWrite, ownsHandle: true);
            var data = Encoding.Default.GetBytes("Written" + Environment.NewLine);
            fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
            fileStream.SetLength(fileStream.Length + data.Length);
            fileStream.Write(data, 0, data.Length);
            fileStream.Flush();

            transactionScope.Complete();

            NativeMethods.CloseHandle(filePtr);
            fileStream.Close();

            Assert.Equal("Original contents" + Environment.NewLine
                + "Written" + Environment.NewLine, txFileSystem.File.ReadAllText(fileName));

            txFileSystem.File.Delete(fileName);
        }

        [Fact]
        [Obsolete("Eventhough FileStream.Create(IntPtr handle, FileAccess access, bool ownsHandle) is deprecated it is still part of the interface")]
        public void CreateFromFileHandleOperation_OwnsHandle_ExceptionThrown_ContentsUnchanged()
        {
            var fileSystem = new FileSystem();
            var txFileSystem = new TxFileSystem(fileSystem);

            string fileName = UnitTestUtils.GetTempFileName(txFileSystem);

            var streamWriter = txFileSystem.File.CreateText(fileName);
            streamWriter.Write("Original contents" + Environment.NewLine);
            streamWriter.Flush();
            streamWriter.Close();

            var filePtr = NativeMethods.CreateFile(fileName, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero,
                FileMode.OpenOrCreate, FileAttributes.Normal, IntPtr.Zero);

            Stream fileStream = null;
            var data = Encoding.Default.GetBytes("Written" + Environment.NewLine);

            Assert.ThrowsAny<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();

                txFileSystem = new TxFileSystem(fileSystem);
                fileStream = txFileSystem.FileStream.Create(filePtr, FileAccess.ReadWrite, ownsHandle: true);
                fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
                fileStream.SetLength(fileStream.Length + data.Length);
                fileStream.Write(data, 0, data.Length);
                fileStream.Flush();

                throw new Exception("Error occurred right after writing to stream");
            });

            NativeMethods.CloseHandle(filePtr);
            fileStream.Close();

            Assert.Equal("Original contents" + Environment.NewLine, txFileSystem.File.ReadAllText(fileName));

            txFileSystem.File.Delete(fileName);
        }

        [Fact, FsFact]
        [Obsolete("Eventhough FileStream.Create(IntPtr handle, FileAccess access, bool ownsHandle) is deprecated it is still part of the interface")]
        public void CreateFromFileHandleOperation_OwnsHandle_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var filePtr = new IntPtr(1234);
            var fileAccess = FileAccess.ReadWrite;
            var ownsHandle = false;

            var fileStreamMock = new Mock<FileStream>(MockBehavior.Loose, filePtr, fileAccess, ownsHandle);

            fileSystemMock.Setup(f => f.FileStream.Create(It.Is<IntPtr>((p) => p == filePtr),
                It.Is<FileAccess>((a) => a == fileAccess), It.Is<bool>((o) => o == ownsHandle)))
                .Returns(fileStreamMock.Object);

            var fileStreamReturned = txFileSystem.FileStream.Create(filePtr, fileAccess, ownsHandle);

            fileSystemMock.Verify(f => f.FileStream.Create(It.Is<IntPtr>((p) => p == filePtr),
                It.Is<FileAccess>((a) => a == fileAccess), It.Is<bool>((o) => o == ownsHandle)), Times.Once);

            Assert.Equal(fileStreamMock.Object, fileStreamReturned);
        }

        [Fact, FsFact]
        [Obsolete("Eventhough FileStream.Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize) is deprecated it is still part of the interface")]
        public void CreateFromFileHandleOperation_OwnsHandleAndBufferSize_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var filePtr = new IntPtr(1234);
            var fileAccess = FileAccess.ReadWrite;
            var ownsHandle = false;
            var bufferSize = 512;

            var fileStreamMock = new Mock<FileStream>(MockBehavior.Loose, filePtr, fileAccess, ownsHandle, bufferSize);

            fileSystemMock.Setup(f => f.FileStream.Create(It.Is<IntPtr>((p) => p == filePtr),
                It.Is<FileAccess>((a) => a == fileAccess), It.Is<bool>((o) => o == ownsHandle),
                It.Is<int>((s) => s == bufferSize)))
                .Returns(fileStreamMock.Object);

            var fileStreamReturned = txFileSystem.FileStream.Create(filePtr, fileAccess, ownsHandle, bufferSize);

            fileSystemMock.Verify(f => f.FileStream.Create(It.Is<IntPtr>((p) => p == filePtr),
                It.Is<FileAccess>((a) => a == fileAccess), It.Is<bool>((o) => o == ownsHandle),
                It.Is<int>((s) => s == bufferSize)), Times.Once);

            Assert.Equal(fileStreamMock.Object, fileStreamReturned);
        }

        [Fact, FsFact]
        [Obsolete("Eventhough FileStream.Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync) is deprecated it is still part of the interface")]
        public void CreateFromFileHandleOperation_OwnsHandleAndBufferSizeAndIsAsync_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var filePtr = new IntPtr(1234);
            var fileAccess = FileAccess.ReadWrite;
            var ownsHandle = false;
            var bufferSize = 512;
            var isAsync = false;

            var fileStreamMock = new Mock<FileStream>(MockBehavior.Loose, filePtr, fileAccess,
                ownsHandle, bufferSize, isAsync);

            fileSystemMock.Setup(f => f.FileStream.Create(It.Is<IntPtr>((p) => p == filePtr),
                It.Is<FileAccess>((a) => a == fileAccess), It.Is<bool>((o) => o == ownsHandle),
                It.Is<int>((s) => s == bufferSize), It.Is<bool>((a) => a == isAsync)))
                .Returns(fileStreamMock.Object);

            var fileStreamReturned = txFileSystem.FileStream.Create(filePtr, fileAccess,
                ownsHandle, bufferSize, isAsync);

            fileSystemMock.Verify(f => f.FileStream.Create(It.Is<IntPtr>((p) => p == filePtr),
                It.Is<FileAccess>((a) => a == fileAccess), It.Is<bool>((o) => o == ownsHandle),
                It.Is<int>((s) => s == bufferSize), It.Is<bool>((a) => a == isAsync)), Times.Once);

            Assert.Equal(fileStreamMock.Object, fileStreamReturned);
        }
    }
}
