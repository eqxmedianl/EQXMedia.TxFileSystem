namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Text;
    using System.Transactions;
    using Xunit;

    public sealed class AppendAllTextOperation_Should
    {
        [Fact]
        public void AppendAllTextOperation_Transactional_AllText_EqualsAppendedText()
        {
            TxFileSystem txFileSystem;

            var mockFileSystem = new MockFileSystem();
            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendtextto.txt");

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllText("/tmp/filetoappendtextto.txt", GetLoremIpsumText());
                transactionScope.Complete();
            }

            var txJournal = txFileSystem.Journal;

            Assert.False(txJournal.IsRolledBack);
            Assert.Equal(GetLoremIpsumText(), txFileSystem.File.ReadAllText("/tmp/filetoappendtextto.txt"));
        }

        [Fact]
        public void AppendAllTextOperation_ExceptionThrown_AllText_EqualsEmpty()
        {
            TxFileSystem txFileSystem;

            var mockFileSystem = new MockFileSystem();
            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendtextto.txt");

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.File.AppendAllText("/tmp/filetoappendtextto.txt", GetLoremIpsumText());

                    throw new Exception("Error occurred right after appending text");
                }
            });

            var txJournal = txFileSystem.Journal;

            Assert.True(txJournal.IsRolledBack);
            Assert.Equal(string.Empty, txFileSystem.File.ReadAllText("/tmp/filetoappendtextto.txt"));
        }

        [Fact]
        public void AppendAllTextOperation_EncodedAsAsccii_AllText_NotEqualsAppendedText()
        {
            TxFileSystem txFileSystem;

            var mockFileSystem = new MockFileSystem();
            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendtextto.txt");

            using (var transactionScope = new TransactionScope())
            {    
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllText("/tmp/filetoappendtextto.txt", GetRandomlyGeneratedFrenchText(),
                    Encoding.ASCII);
                transactionScope.Complete();
            }

            var txJournal = txFileSystem.Journal;

            Assert.False(txJournal.IsRolledBack);
            Assert.NotEqual(GetRandomlyGeneratedFrenchText(),
                txFileSystem.File.ReadAllText("/tmp/filetoappendtextto.txt"));
        }

        [Fact]
        public void AppendAllTextOperation_EncodedAsUtf8_AllText_EqualsAppendedText()
        {
            TxFileSystem txFileSystem;

            var mockFileSystem = new MockFileSystem();
            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendtextto.txt");

            using (var transactionScope = new TransactionScope())
            {    
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllText("/tmp/filetoappendtextto.txt", GetRandomlyGeneratedFrenchText(),
                    Encoding.UTF8);
                transactionScope.Complete();
            }

            var txJournal = txFileSystem.Journal;

            Assert.False(txJournal.IsRolledBack);
            Assert.Equal(GetRandomlyGeneratedFrenchText(), txFileSystem.File.ReadAllText("/tmp/filetoappendtextto.txt"));
        }

#if ASYNC_IO
        [Fact]
        public void AppendAllTextOperationAsync_Transactional_AllText_EqualsAppendedText()
        {
            TxFileSystem txFileSystem;

            var mockFileSystem = new MockFileSystem();
            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendtextto.txt");

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllTextAsync("/tmp/filetoappendtextto.txt", GetLoremIpsumText()).Wait();
                transactionScope.Complete();
            }

            var txJournal = txFileSystem.Journal;

            Assert.False(txJournal.IsRolledBack);
            Assert.Equal(GetLoremIpsumText(), txFileSystem.File.ReadAllText("/tmp/filetoappendtextto.txt"));
        }

        [Fact]
        public void AppendAllTextOperationAsync_ExceptionThrown_AllText_EqualsEmpty()
        {
            TxFileSystem txFileSystem;

            var mockFileSystem = new MockFileSystem();
            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendtextto.txt");

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.File.AppendAllTextAsync("/tmp/filetoappendtextto.txt", GetLoremIpsumText()).Wait();

                    throw new Exception("Error occurred right after appending text");
                }
            });

            var txJournal = txFileSystem.Journal;

            Assert.True(txJournal.IsRolledBack);
            Assert.Equal(string.Empty, txFileSystem.File.ReadAllText("/tmp/filetoappendtextto.txt"));
        }

        [Fact]
        public void AppendAllTextOperationAsync_EncodedAsAsccii_AllText_NotEqualsAppendedText()
        {
            TxFileSystem txFileSystem;

            var mockFileSystem = new MockFileSystem();
            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendtextto.txt");

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllTextAsync("/tmp/filetoappendtextto.txt", GetRandomlyGeneratedFrenchText(),
                    Encoding.ASCII).Wait();
                transactionScope.Complete();
            }

            var txJournal = txFileSystem.Journal;

            Assert.False(txJournal.IsRolledBack);
            Assert.NotEqual(GetRandomlyGeneratedFrenchText(),
                txFileSystem.File.ReadAllText("/tmp/filetoappendtextto.txt"));
        }

        [Fact]
        public void AppendAllTextOperationAsync_EncodedAsUtf8_AllText_EqualsAppendedText()
        {
            TxFileSystem txFileSystem;

            var mockFileSystem = new MockFileSystem();
            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendtextto.txt");

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.AppendAllTextAsync("/tmp/filetoappendtextto.txt", GetRandomlyGeneratedFrenchText(),
                    Encoding.UTF8).Wait();
                transactionScope.Complete();
            }

            var txJournal = txFileSystem.Journal;

            Assert.False(txJournal.IsRolledBack);
            Assert.Equal(GetRandomlyGeneratedFrenchText(), txFileSystem.File.ReadAllText("/tmp/filetoappendtextto.txt"));
        }
#endif

        private static string GetRandomlyGeneratedFrenchText()
        {
            return @"
Disruptif sur bière épicé même croissant dans omelette. Du coup guillotine guillotine carrément. Devoir fromage suite guillotine si à la pour quand même putain.
";
        }

        private static string GetLoremIpsumText()
        {
            return @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas ac nisi quis est semper interdum a at ligula. Fusce aliquet mauris at lacus ornare, eu fermentum metus eleifend. Sed fringilla pretium felis, ut convallis tortor dignissim ac. Donec dapibus, enim quis gravida dignissim, elit purus dictum dolor, facilisis rutrum ante eros id enim. Nam nulla risus, viverra egestas feugiat sit amet, pellentesque vitae ante. Nam efficitur, lacus in condimentum fringilla, tellus elit vehicula leo, at dictum erat turpis id purus. Donec ac nunc quis metus scelerisque bibendum. Aenean faucibus sapien et ligula ornare, a bibendum turpis interdum. Nunc mollis ultricies ante vel bibendum. In hac habitasse platea dictumst. Nullam sit amet augue feugiat, vestibulum urna et, imperdiet arcu. Integer vitae libero ac erat pulvinar blandit tempus nec enim. Maecenas placerat sit amet massa in tincidunt. Donec molestie placerat libero eget tempus. Donec sed nisi magna.

Aliquam blandit velit ultricies, dignissim massa id, dapibus purus. Cras tempor feugiat tortor, at hendrerit ex. Phasellus sed enim ac justo consequat facilisis. Donec tellus magna, vulputate non varius sed, pellentesque congue sapien. Praesent tincidunt viverra tincidunt. Maecenas nec vulputate lectus. Suspendisse purus sapien, lacinia ut massa at, iaculis lobortis felis.
";
        }
    }
}
