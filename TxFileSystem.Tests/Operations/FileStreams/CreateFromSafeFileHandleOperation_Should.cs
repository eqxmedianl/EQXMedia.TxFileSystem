namespace EQXMedia.TxFileSystem.Tests.Operations.FileStreams
{
    using global::EQXMedia.TxFileSystem.NativeMethods.Win32;
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using global::EQXMedia.TxFileSystem.Tests.Operations.FileStreams.Utils;
    using Microsoft.Win32.SafeHandles;
    using Moq;
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Text;
    using System.Transactions;
    using Xunit;
    using FileAttributes = global::EQXMedia.TxFileSystem.NativeMethods.Win32.FileAttributes;

    public sealed class CreateFromSafeFileHandleOperation_Should
    {
        [Fact]
        public void CreateFromSafeFileHandleOperation_ContentsChanged()
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
            var safeFileHandle = new SafeFileHandle(filePtr, ownsHandle: true);

            Stream fileStream = null;

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(fileSystem);
                fileStream = txFileSystem.FileStream.Create(safeFileHandle, FileAccess.ReadWrite);
                var data = Encoding.Default.GetBytes("Written" + Environment.NewLine);
                fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
                fileStream.SetLength(fileStream.Length + data.Length);
                fileStream.Write(data, 0, data.Length);
                fileStream.Flush();

                transactionScope.Complete();
            }

            NativeMethods.CloseHandle(filePtr);
            fileStream.Close();

            Assert.Equal("Original contents" + Environment.NewLine
                + "Written" + Environment.NewLine, txFileSystem.File.ReadAllText(fileName));

            txFileSystem.File.Delete(fileName);
        }

        [Fact]
        public void CreateFromSafeFileHandleOperation_ExceptionThrown_ContentsUnchanged()
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
            var safeFileHandle = new SafeFileHandle(filePtr, ownsHandle: true);

            Stream fileStream = null;
            var data = Encoding.Default.GetBytes("Written" + Environment.NewLine);

            Assert.ThrowsAny<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(fileSystem);
                    fileStream = txFileSystem.FileStream.Create(safeFileHandle, FileAccess.ReadWrite);
                    fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
                    fileStream.SetLength(fileStream.Length + data.Length);
                    fileStream.Write(data, 0, data.Length);
                    fileStream.Flush();
                }

                throw new Exception("Error occurred right after writing to stream");
            });

            NativeMethods.CloseHandle(filePtr);
            fileStream.Close();

            Assert.Equal("Original contents" + Environment.NewLine, txFileSystem.File.ReadAllText(fileName));

            txFileSystem.File.Delete(fileName);
        }

        [Fact]
        public void CreateFromSafeFileHandleOperation_DoesntOwnHandle_ContentsChanged()
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
            var safeFileHandle = new SafeFileHandle(filePtr, ownsHandle: false);

            Stream fileStream = null;

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(fileSystem);
                fileStream = txFileSystem.FileStream.Create(safeFileHandle, FileAccess.ReadWrite);
                var data = Encoding.Default.GetBytes("Written" + Environment.NewLine);
                fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
                fileStream.SetLength(fileStream.Length + data.Length);
                fileStream.Write(data, 0, data.Length);
                fileStream.Flush();

                transactionScope.Complete();
            }

            NativeMethods.CloseHandle(filePtr);
            fileStream.Close();

            Assert.Equal("Original contents" + Environment.NewLine
                + "Written" + Environment.NewLine, txFileSystem.File.ReadAllText(fileName));

            txFileSystem.File.Delete(fileName);
        }

        [Fact]
        public void CreateFromSafeFileHandleOperation_DoesntOwnHandle_ExceptionThrown_ContentsUnchanged()
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
            var safeFileHandle = new SafeFileHandle(filePtr, ownsHandle: false);

            Stream fileStream = null;
            var data = Encoding.Default.GetBytes("Written" + Environment.NewLine);

            Assert.ThrowsAny<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(fileSystem);
                    fileStream = txFileSystem.FileStream.Create(safeFileHandle, FileAccess.ReadWrite);
                    fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
                    fileStream.SetLength(fileStream.Length + data.Length);
                    fileStream.Write(data, 0, data.Length);
                    fileStream.Flush();

                    throw new Exception("Error occurred right after writing to stream");
                }
            });

            NativeMethods.CloseHandle(filePtr);
            fileStream.Close();

            Assert.Equal("Original contents" + Environment.NewLine, txFileSystem.File.ReadAllText(fileName));

            txFileSystem.File.Delete(fileName);
        }

        [Fact, FsFact]
        public void CreateFromSafeFileHandleOperation_DoesntOwnHandle_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var ownsHandle = false;
            var fileAccess = FileAccess.ReadWrite;
            var fileName = UnitTestUtils.GetTempFileName(new TxFileSystem());
            var filePtr = NativeMethods.CreateFile(fileName, fileAccess, FileShare.ReadWrite, IntPtr.Zero,
                FileMode.OpenOrCreate, FileAttributes.Normal, IntPtr.Zero);
            var safeFileHandle = new SafeFileHandle(filePtr, ownsHandle: ownsHandle);

            var fileStreamMock = new Mock<FileStream>(MockBehavior.Loose, safeFileHandle, fileAccess);

            fileSystemMock.Setup(f => f.FileStream.Create(It.Is<SafeFileHandle>((p) => p == safeFileHandle),
                It.Is<FileAccess>((a) => a == fileAccess)))
                .Returns(fileStreamMock.Object);

            var fileStreamReturned = txFileSystem.FileStream.Create(safeFileHandle, fileAccess);

            fileSystemMock.Verify(f => f.FileStream.Create(It.Is<SafeFileHandle>((p) => p == safeFileHandle),
                It.Is<FileAccess>((a) => a == fileAccess)), Times.Once);

            Assert.Equal(fileStreamMock.Object, fileStreamReturned);

            NativeMethods.CloseHandle(filePtr);
            new TxFileSystem().File.Delete(fileName);
        }

        [Fact, FsFact]
        public void CreateFromSafeFileHandleOperation_BufferSize_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var ownsHandle = true;
            var fileAccess = FileAccess.ReadWrite;
            var fileName = UnitTestUtils.GetTempFileName(new TxFileSystem());
            var filePtr = NativeMethods.CreateFile(fileName, fileAccess, FileShare.ReadWrite, IntPtr.Zero,
                FileMode.OpenOrCreate, FileAttributes.Normal, IntPtr.Zero);
            var safeFileHandle = new SafeFileHandle(filePtr, ownsHandle: ownsHandle);
            var bufferSize = 512;

            var fileStreamMock = new Mock<FileStream>(MockBehavior.Loose, safeFileHandle, fileAccess, bufferSize);

            fileSystemMock.Setup(f => f.FileStream.Create(It.Is<SafeFileHandle>((p) => p == safeFileHandle),
                It.Is<FileAccess>((a) => a == fileAccess), It.Is<int>((s) => s == bufferSize)))
                .Returns(fileStreamMock.Object);

            var fileStreamReturned = txFileSystem.FileStream.Create(safeFileHandle, fileAccess, bufferSize);

            fileSystemMock.Verify(f => f.FileStream.Create(It.Is<SafeFileHandle>((p) => p == safeFileHandle),
                It.Is<FileAccess>((a) => a == fileAccess), It.Is<int>((s) => s == bufferSize)), Times.Once);

            Assert.Equal(fileStreamMock.Object, fileStreamReturned);

            NativeMethods.CloseHandle(filePtr);
            new TxFileSystem().File.Delete(fileName);
        }

        [Fact, FsFact]
        public void CreateFromSafeFileHandleOperation_BufferSizeAndIsAsync_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var ownsHandle = true;
            var fileAccess = FileAccess.ReadWrite;
            var fileName = UnitTestUtils.GetTempFileName(new TxFileSystem());
            var filePtr = NativeMethods.CreateFile(fileName, fileAccess, FileShare.ReadWrite, IntPtr.Zero,
                FileMode.OpenOrCreate, FileAttributes.Normal, IntPtr.Zero);
            var safeFileHandle = new SafeFileHandle(filePtr, ownsHandle: ownsHandle);
            var bufferSize = 512;
            var isAsync = false;

            var fileStreamMock = new Mock<FileStream>(MockBehavior.Loose, safeFileHandle, fileAccess,
                bufferSize, isAsync);

            fileSystemMock.Setup(f => f.FileStream.Create(It.Is<SafeFileHandle>((p) => p == safeFileHandle),
                It.Is<FileAccess>((a) => a == fileAccess), It.Is<int>((s) => s == bufferSize),
                It.Is<bool>((a) => a == isAsync)))
                .Returns(fileStreamMock.Object);

            var fileStreamReturned = txFileSystem.FileStream.Create(safeFileHandle, fileAccess, bufferSize, isAsync);

            fileSystemMock.Verify(f => f.FileStream.Create(It.Is<SafeFileHandle>((p) => p == safeFileHandle),
                It.Is<FileAccess>((a) => a == fileAccess), It.Is<int>((s) => s == bufferSize),
                It.Is<bool>((a) => a == isAsync)), Times.Once);

            Assert.Equal(fileStreamMock.Object, fileStreamReturned);

            NativeMethods.CloseHandle(filePtr);
            new TxFileSystem().File.Delete(fileName);
        }
    }
}
