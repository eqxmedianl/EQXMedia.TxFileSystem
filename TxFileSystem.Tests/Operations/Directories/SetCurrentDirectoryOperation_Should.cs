namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class SetCurrentDirectoryOperation_Should
    {
        [Fact]
        public void SetCurrentDirectoryOperation_ChangesCurrentDirectory()
        {
            string oldDirPath;
            var newDirPath = "/tmp/newdir";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                oldDirPath = txFileSystem.Directory.GetCurrentDirectory();
                txFileSystem.Directory.SetCurrentDirectory(newDirPath);

                transactionScope.Complete();
            }

            Assert.NotEqual(oldDirPath, txFileSystem.Directory.GetCurrentDirectory());
            Assert.Equal(newDirPath, txFileSystem.Directory.GetCurrentDirectory());
        }

        [Fact]
        public void SetCurrentDirectoryOperation_ExceptionThrown_CurrentDirectoryUnchanged()
        {
            string oldDirPath = null;
            var newDirPath = "/tmp/newdir";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            Assert.ThrowsAny<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    oldDirPath = txFileSystem.Directory.GetCurrentDirectory();
                    txFileSystem.Directory.SetCurrentDirectory(newDirPath);

                    throw new Exception("Error occurred right after changing directory");
                }
            });

            Assert.NotEqual(newDirPath, txFileSystem.Directory.GetCurrentDirectory());
            Assert.Equal(oldDirPath, txFileSystem.Directory.GetCurrentDirectory());
        }
    }
}
