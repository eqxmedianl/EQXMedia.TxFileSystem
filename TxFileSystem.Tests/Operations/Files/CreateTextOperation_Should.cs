namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using System;
    using System.IO;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class CreateTextOperation_Should
    {
        [Fact]
        public void CreateTextOperation_ReturnsStreamWriter()
        {
            var fileName = "/tmp/generatedfile.txt";

            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var streamWriter = txFileSystem.File.CreateText(fileName);

            transactionScope.Complete();

            Assert.IsAssignableFrom<StreamWriter>(streamWriter);
        }

        [Fact]
        public void CreateTextOperation_File_Exists()
        {
            var fileName = "/tmp/generatedfile.txt";

            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            txFileSystem.File.CreateText(fileName);

            transactionScope.Complete();

            Assert.True(txFileSystem.File.Exists(fileName));
        }

        [Fact]
        public void CreateTextOperation_File_ContainsText()
        {
            var fileName = "/tmp/generatedfile.txt";

            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var streamWriter = txFileSystem.File.CreateText(fileName);
            streamWriter.Write(GetLoremIpsumText());
            streamWriter.Flush();
            streamWriter.Close();

            transactionScope.Complete();

            Assert.Equal(GetLoremIpsumText(), txFileSystem.File.ReadAllText(fileName));
        }

        [Fact]
        public void CreateTextOperation_ExceptionThrown_NoFileCreated()
        {
            var fileName = "/tmp/generatedfile.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();
                txFileSystem = new TxFileSystem(mockFileSystem);

                var streamWriter = txFileSystem.File.CreateText(fileName);
                streamWriter.Write(GetLoremIpsumText());
                streamWriter.Flush();
                streamWriter.Close();

                throw new Exception("Error right after writing to stream");
            });

            Assert.False(txFileSystem.File.Exists(fileName));
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
