namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class MoveOperation_Should
    {
        [Fact]
        public void MoveOperation_ProperlyMoved()
        {
            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var textStream = txFileSystem.File.CreateText("/tmp/sourcefile.txt");
            textStream.Write("Source file contents");
            textStream.Flush();
            textStream.Close();

            txFileSystem.File.Move("/tmp/sourcefile.txt", "/tmp/destfile.txt");

            transactionScope.Complete();

            Assert.False(txFileSystem.File.Exists("/tmp/sourcefile.txt"));
            Assert.True(txFileSystem.File.Exists("/tmp/destfile.txt"));
            Assert.Equal("Source file contents", txFileSystem.File.ReadAllText("/tmp/destfile.txt"));
        }

        [Fact]
        public void MoveOperation_ExceptionThrown_DestFile_DoesntExist()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create("/tmp/sourcefile.txt");

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();

                var txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.Move("/tmp/sourcefile.txt", "/tmp/destfile.txt");

                throw new Exception("Error occurred right after moving file");
            });

            Assert.False(txFileSystem.File.Exists("/tmp/destfile.txt"));
        }

        [Fact]
        public void MoveOperation_ExceptionThrown_SourceFile_RestoredProperly()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            
            var textStream = txFileSystem.File.CreateText("/tmp/sourcefile.txt");
            textStream.Write("Source file contents");
            textStream.Flush();
            textStream.Close();

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();

                var txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.Move("/tmp/sourcefile.txt", "/tmp/destfile.txt");

                throw new Exception("Error occurred right after moving file");
            });

            Assert.True(txFileSystem.File.Exists("/tmp/sourcefile.txt"));
            Assert.Equal("Source file contents", txFileSystem.File.ReadAllText("/tmp/sourcefile.txt"));
        }
    }
}
