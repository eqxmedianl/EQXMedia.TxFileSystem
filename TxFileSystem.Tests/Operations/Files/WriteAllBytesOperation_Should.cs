namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Text;
    using System.Transactions;
    using Xunit;

    public sealed class WriteAllBytesOperation_Should
    {
        [Fact]
        public void WriteAllBytesOperation_ExceptionThrown_KeepsFile()
        {
            var data = Encoding.ASCII.GetBytes("Byte array text contents");

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/bytesfile.txt");

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();

                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.WriteAllBytes("/tmp/bytesfile.txt", data);

                throw new Exception("Error occurred right after writing bytes");
            });

            Assert.True(txFileSystem.File.Exists("/tmp/bytesfile.txt"));
            Assert.Equal(Encoding.ASCII.GetBytes(""), txFileSystem.File.ReadAllBytes("/tmp/bytesfile.txt"));
        }

        [Fact]
        public void WriteAllBytesOperation_BytesWritten()
        {
            var data = Encoding.ASCII.GetBytes("Byte array text contents");

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/bytesfile.txt");

            using var transactionScope = new TransactionScope();

            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.File.WriteAllBytes("/tmp/bytesfile.txt", data);

            transactionScope.Complete();

            Assert.True(txFileSystem.File.Exists("/tmp/bytesfile.txt"));
            Assert.Equal(data, txFileSystem.File.ReadAllBytes("/tmp/bytesfile.txt"));
        }

        [Fact]
        public void WriteAllBytesOperationAsync_ExceptionThrown_KeepsFile()
        {
            var data = Encoding.ASCII.GetBytes("Byte array text contents");

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/bytesfile.txt");

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();

                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.WriteAllBytesAsync("/tmp/bytesfile.txt", data).Wait();

                throw new Exception("Error occurred right after writing bytes");
            });

            Assert.True(txFileSystem.File.Exists("/tmp/bytesfile.txt"));
            Assert.Equal(Encoding.ASCII.GetBytes(""), txFileSystem.File.ReadAllBytes("/tmp/bytesfile.txt"));
        }

        [Fact]
        public void WriteAllBytesOperationAsync_BytesWritten()
        {
            var data = Encoding.ASCII.GetBytes("Byte array text contents");

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/bytesfile.txt");

            using var transactionScope = new TransactionScope();

            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.File.WriteAllBytesAsync("/tmp/bytesfile.txt", data).Wait();

            transactionScope.Complete();

            Assert.True(txFileSystem.File.Exists("/tmp/bytesfile.txt"));
            Assert.Equal(data, txFileSystem.File.ReadAllBytes("/tmp/bytesfile.txt"));
        }
    }
}
