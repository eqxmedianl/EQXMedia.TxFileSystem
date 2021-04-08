namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class AppendTextOperation_Should
    {
        [Fact]
        public void AppendTextOperation_Transactional_AllText_EqualsInitialPlusAdded()
        {
            TxFileSystem txFileSystem;

            var mockFileSystem = new MockFileSystem();
            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendtextto.txt");
            var textFileStream = txFileSystem.File.AppendText("/tmp/filetoappendtextto.txt");
            textFileStream.WriteLine("Initial contents");
            textFileStream.Flush();
            textFileStream.Close();

            using var transactionScope = new TransactionScope();
            txFileSystem = new TxFileSystem(mockFileSystem);

            textFileStream = txFileSystem.File.AppendText("/tmp/filetoappendtextto.txt");
            textFileStream.Write(GetLoremIpsumText());
            textFileStream.Flush();
            textFileStream.Close();

            transactionScope.Complete();

            var txJournal = ((ITxFileSystem)txFileSystem).Journal;

            Assert.False(txJournal.IsRolledBack);
            Assert.Equal(
                "Initial contents" + Environment.NewLine + GetLoremIpsumText(),
                txFileSystem.File.ReadAllText("/tmp/filetoappendtextto.txt")
            );
        }

        [Fact]
        public void AppendTextOperation_ExceptionThrown_AllText_EqualsInitial()
        {
            TxFileSystem txFileSystem;

            var mockFileSystem = new MockFileSystem();
            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText("/tmp/filetoappendtextto.txt");
            var textFileStream = txFileSystem.File.AppendText("/tmp/filetoappendtextto.txt");
            textFileStream.WriteLine("Initial contents");
            textFileStream.Flush();
            textFileStream.Close();

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();
                txFileSystem = new TxFileSystem(mockFileSystem);

                textFileStream = txFileSystem.File.AppendText("/tmp/filetoappendtextto.txt");
                textFileStream.Write(GetRandomlyGeneratedFrenchText());
                textFileStream.Flush();
                textFileStream.Close();

                throw new Exception("Error occurred right after appending text");
            });

            var txJournal = ((ITxFileSystem)txFileSystem).Journal;

            Assert.True(txJournal.IsRolledBack);
            Assert.Equal("Initial contents" + Environment.NewLine, txFileSystem.File.ReadAllText("/tmp/filetoappendtextto.txt")
            );
        }

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
