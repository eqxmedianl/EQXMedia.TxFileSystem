namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using System;
#if NETCOREAPP3_1_OR_GREATER
    using System.Diagnostics.CodeAnalysis;
#endif
    using System.IO;
    using System.IO.Abstractions.TestingHelpers;
    using System.Text;
    using System.Transactions;
    using Xunit;

    public sealed class OpenWriteOperation_Should
    {
        [Fact]
#if NETCOREAPP3_1_OR_GREATER
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void OpenWriteOperation_RetunsStreamReader()
        {
            var fileName = "/tmp/filetwriteinto.txt";

            var mockFileSystem = new MockFileSystem();

            using (var transactionScope = new TransactionScope())
            {
                var txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.Directory.CreateDirectory("/tmp");
                txFileSystem.File.CreateText(fileName);

                var fileStream = txFileSystem.File.OpenWrite(fileName);

                transactionScope.Complete();

                Assert.IsAssignableFrom<Stream>(fileStream);
            }
        }

        [Fact]
#if NETCOREAPP3_1_OR_GREATER
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void OpenWriteOperation_AddedToJournal()
        {
            var fileName = "/tmp/filetwriteinto.txt";

            var mockFileSystem = new MockFileSystem();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.OpenWrite(fileName);

                Assert.NotEmpty(txFileSystem.Journal._txJournalEntries);
            }
        }

        [Fact]
#if NETCOREAPP3_1_OR_GREATER
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void OpenWriteOperation_ExceptionThrown_DataNotWritten()
        {
            var fileName = "/tmp/filetwriteinto.txt";
            var data = Encoding.ASCII.GetBytes("Bytes written to file");

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);

            Assert.ThrowsAny<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);

                    var fileStream = txFileSystem.File.OpenWrite(fileName);
                    fileStream.Write(data, 0, data.Length);
                    fileStream.Flush();
                    fileStream.Close();

                    throw new Exception("Error occurred right after writing to stream");
                }
            });

            Assert.Equal(Array.Empty<byte>(), txFileSystem.File.ReadAllBytes(fileName));
        }

        [Fact]
        public void OpenWriteOperation_DataWritten()
        {
            var fileName = "/tmp/filetwriteinto.txt";
            var data = Encoding.ASCII.GetBytes("Bytes written to file");

            var mockFileSystem = new MockFileSystem();
            TxFileSystem txFileSystem = null;

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.Directory.CreateDirectory("/tmp");
                txFileSystem.File.CreateText(fileName);

                var fileStream = txFileSystem.File.OpenWrite(fileName);
                fileStream.Write(data, 0, data.Length);
                fileStream.Flush();
                fileStream.Close();

                transactionScope.Complete();
            }

            Assert.Equal(data, txFileSystem.File.ReadAllBytes(fileName));
        }
    }
}
