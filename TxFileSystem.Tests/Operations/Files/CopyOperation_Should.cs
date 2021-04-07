namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class CopyOperation_Should
    {
        [Fact]
        public void CopyOperation_Creates_EqualFile()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            using var transactionScope = new TransactionScope();

            var textFileStream = txFileSystem.File.CreateText("/tmp/createdfile.txt");
            textFileStream.Write("Newly created file which is going to be copied");
            textFileStream.Flush();
            textFileStream.Close();

            txFileSystem.File.Copy("/tmp/createdfile.txt", "/tmp/copied.txt");

            transactionScope.Complete();

            Assert.Equal(txFileSystem.File.ReadAllText("/tmp/createdfile.txt"),
                txFileSystem.File.ReadAllText("/tmp/copied.txt"));
        }

        [Fact]
        public void CopyOperation_ExceptionThrown_DestitionFileFound_ReturnsFalse()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var textFileStream = txFileSystem.File.CreateText("/tmp/createdfile.txt");
            textFileStream.Write("Newly created file which is going to be copied");
            textFileStream.Flush();
            textFileStream.Close();

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.Copy("/tmp/createdfile.txt", "/tmp/copied.txt");

                throw new Exception("Error right after copying file.");
            });

            Assert.True(txFileSystem.File.Exists("/tmp/createdfile.txt"));
            Assert.False(txFileSystem.File.Exists("/tmp/copied.txt"));
        }

        [Fact]
        public void CopyOperation_Overwrites_DestFile()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            using var transactionScope = new TransactionScope();

            txFileSystem.File.CreateText("/tmp/destfile.txt");

            var textFileStream = txFileSystem.File.CreateText("/tmp/createdfile.txt");
            textFileStream.Write("Newly created file which is going to be copied");
            textFileStream.Flush();
            textFileStream.Close();

            txFileSystem.File.Copy("/tmp/createdfile.txt", "/tmp/destfile.txt", overwrite: true);

            transactionScope.Complete();

            Assert.Equal(txFileSystem.File.ReadAllText("/tmp/createdfile.txt"),
                txFileSystem.File.ReadAllText("/tmp/destfile.txt"));
        }

        [Fact]
        public void CopyOperation_ExceptionThrown_RestoresDestfile()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var textFileStream = txFileSystem.File.CreateText("/tmp/destfile.txt");
            textFileStream.Write("Dest file contents");
            textFileStream.Flush();
            textFileStream.Close();

            textFileStream = txFileSystem.File.CreateText("/tmp/createdfile.txt");
            textFileStream.Write("Newly created file which is going to be copied");
            textFileStream.Flush();
            textFileStream.Close();

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();
                var txFileSystem = new TxFileSystem(mockFileSystem);

                txFileSystem.File.Copy("/tmp/createdfile.txt", "/tmp/destfile.txt", overwrite: true);

                throw new Exception("Error occurred right after copy");
            });

            Assert.NotEqual(txFileSystem.File.ReadAllText("/tmp/createdfile.txt"),
                txFileSystem.File.ReadAllText("/tmp/destfile.txt"));
            Assert.Equal("Dest file contents", txFileSystem.File.ReadAllText("/tmp/destfile.txt"));
        }

        [Fact]
        public void CopyOperation_ExceptionThrown_DeletesDestfile()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var textFileStream = txFileSystem.File.CreateText("/tmp/createdfile.txt");
            textFileStream.Write("Newly created file which is going to be copied");
            textFileStream.Flush();
            textFileStream.Close();

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();
                var txFileSystem = new TxFileSystem(mockFileSystem);

                txFileSystem.File.Copy("/tmp/createdfile.txt", "/tmp/destfile.txt", overwrite: true);

                throw new Exception("Error occurred right after copy");
            });

            Assert.True(txFileSystem.File.Exists("/tmp/createdfile.txt"));
            Assert.False(txFileSystem.File.Exists("/tmp/destfile.txt"));
        }
    }
}
