namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Linq;
    using System.Text;
    using System.Transactions;
    using Xunit;

    public sealed class AppendAllLinesOperation_Should
    {
        [Fact]
        public void AppendAllLinesOperation_EqualsAllWrittenLines()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var lines = new string[] { "line one", "line two", "line three" };

            var textFileStream = txFileSystem.File.CreateText("/tmp/filetoappendto.txt");
            textFileStream.WriteLine("Initial contents");
            textFileStream.Flush();
            textFileStream.Close();

            Assert.Equal("Initial contents" + Environment.NewLine,
                txFileSystem.File.ReadAllText("/tmp/filetoappendto.txt"));

            txFileSystem.File.AppendAllLines("/tmp/filetoappendto.txt", lines.ToList());

            lines = new[] { "Initial contents" }.Concat(lines).ToArray();

            Assert.Equal(lines, txFileSystem.File.ReadAllLines("/tmp/filetoappendto.txt"));
        }

        [Fact]
        public void AppendAllLinesOperation_ThrownException_ContainsInitiallyWrittenLines()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var initialLine = new string[] { "Initial contents" };
            var lines = new string[] { "line one", "line two", "line three" };

            var textFileStream = txFileSystem.File.CreateText("/tmp/filetoappendto.txt");
            textFileStream.WriteLine(initialLine[0]);
            textFileStream.Flush();
            textFileStream.Close();

            Assert.Equal("Initial contents" + Environment.NewLine,
                txFileSystem.File.ReadAllText("/tmp/filetoappendto.txt"));

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.File.AppendAllLines("/tmp/filetoappendto.txt", lines.ToList());

                    throw new Exception("Error occurred right after appending lines");
                }
            });

            Assert.True(txFileSystem.Journal.IsRolledBack);
            Assert.Equal(initialLine, txFileSystem.File.ReadAllLines("/tmp/filetoappendto.txt"));
        }

        [Fact]
        public void AppendAllLinesOperation_Transactional_ContainsAllLines()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var initialLine = new string[] { "Initial contents" };
            var lines = new string[] { "line one", "line two", "line three" };

            var textFileStream = txFileSystem.File.CreateText("/tmp/filetoappendto.txt");
            textFileStream.WriteLine(initialLine[0]);
            textFileStream.Flush();
            textFileStream.Close();

            Assert.Equal("Initial contents" + Environment.NewLine,
                txFileSystem.File.ReadAllText("/tmp/filetoappendto.txt"));

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllLines("/tmp/filetoappendto.txt", lines.ToList());
                transactionScope.Complete();
            }

            lines = new[] { "Initial contents" }.Concat(lines).ToArray();

            Assert.Equal(lines, txFileSystem.File.ReadAllLines("/tmp/filetoappendto.txt"));
        }

        [Fact]
        public void AppendAllLinesOperation_EncodedAsAscii_LinesDontMatch()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendto.txt");

            var lines = new string[] { "Crème brûlée", "Chocolate mouse", "Cheesecake Icecream" };

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllLines("/tmp/filetoappendto.txt", lines.ToList(), Encoding.ASCII);
                transactionScope.Complete();
            }

            Assert.NotEqual(lines, txFileSystem.File.ReadAllLines("/tmp/filetoappendto.txt"));
        }

        [Fact]
        public void AppendAllLinesOperation_EncodedAsUtf8_LinesDoMatch()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendto.txt");

            var lines = new string[] { "Crème brûlée", "Chocolate mouse", "Cheesecake Icecream" };

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllLines("/tmp/filetoappendto.txt", lines.ToList(), Encoding.UTF8);
                transactionScope.Complete();
            }

            Assert.Equal(lines, txFileSystem.File.ReadAllLines("/tmp/filetoappendto.txt"));
        }

#if ASYNC_IO
        [Fact]
        public void AppendAllLinesOperationAsync_EqualsAllWrittenLines()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var lines = new string[] { "line one", "line two", "line three" };

            var textFileStream = txFileSystem.File.CreateText("/tmp/filetoappendto.txt");
            textFileStream.WriteLine("Initial contents");
            textFileStream.Flush();
            textFileStream.Close();

            Assert.Equal("Initial contents" + Environment.NewLine,
                txFileSystem.File.ReadAllText("/tmp/filetoappendto.txt"));

            txFileSystem.File.AppendAllLinesAsync("/tmp/filetoappendto.txt", lines.ToList()).Wait();

            lines = lines.Prepend("Initial contents").ToArray();

            Assert.Equal(lines, txFileSystem.File.ReadAllLines("/tmp/filetoappendto.txt"));
        }

        [Fact]
        public void AppendAllLinesOperationAsync_ThrownException_ContainsInitiallyWrittenLines()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var initialLine = new string[] { "Initial contents" };
            var lines = new string[] { "line one", "line two", "line three" };

            var textFileStream = txFileSystem.File.CreateText("/tmp/filetoappendto.txt");
            textFileStream.WriteLine(initialLine[0]);
            textFileStream.Flush();
            textFileStream.Close();

            Assert.Equal("Initial contents" + Environment.NewLine,
                txFileSystem.File.ReadAllText("/tmp/filetoappendto.txt"));

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.File.AppendAllLinesAsync("/tmp/filetoappendto.txt", lines.ToList()).Wait();

                    throw new Exception("Error occurred right after appending lines");
                }
            });

            Assert.True(txFileSystem.Journal.IsRolledBack);
            Assert.Equal(initialLine, txFileSystem.File.ReadAllLines("/tmp/filetoappendto.txt"));
        }

        [Fact]
        public void AppendAllLinesOperationAsync_Transactional_ContainsAllLines()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var initialLine = new string[] { "Initial contents" };
            var lines = new string[] { "line one", "line two", "line three" };

            var textFileStream = txFileSystem.File.CreateText("/tmp/filetoappendto.txt");
            textFileStream.WriteLine(initialLine[0]);
            textFileStream.Flush();
            textFileStream.Close();

            Assert.Equal("Initial contents" + Environment.NewLine,
                txFileSystem.File.ReadAllText("/tmp/filetoappendto.txt"));

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllLinesAsync("/tmp/filetoappendto.txt", lines.ToList()).Wait();
                transactionScope.Complete();
            }

            lines = lines.Prepend("Initial contents").ToArray();

            Assert.Equal(lines, txFileSystem.File.ReadAllLines("/tmp/filetoappendto.txt"));
        }

        [Fact]
        public void AppendAllLinesOperationAsync_EncodedAsAscii_LinesDontMatch()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendto.txt");

            var lines = new string[] { "Crème brûlée", "Chocolate mouse", "Cheesecake Icecream" };

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllLinesAsync("/tmp/filetoappendto.txt", lines.ToList(), Encoding.ASCII).Wait();
                transactionScope.Complete();
            }

            Assert.NotEqual(lines, txFileSystem.File.ReadAllLines("/tmp/filetoappendto.txt"));
        }

        [Fact]
        public void AppendAllLinesOperationAsync_EncodedAsUtf8_LinesDoMatch()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendto.txt");

            var lines = new string[] { "Crème brûlée", "Chocolate mouse", "Cheesecake Icecream" };

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllLinesAsync("/tmp/filetoappendto.txt", lines.ToList(), Encoding.UTF8).Wait();
                transactionScope.Complete();
            }

            Assert.Equal(lines, txFileSystem.File.ReadAllLines("/tmp/filetoappendto.txt"));
        }
#endif
    }
}
