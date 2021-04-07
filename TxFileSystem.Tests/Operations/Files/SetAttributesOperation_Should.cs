namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Reflection;
    using System.Transactions;
    using Xunit;

    public sealed class SetAttributesOperation_Should
    {
        [Fact]
        public void SetAttributesOperation_AttributesChanged()
        {
            var fileName = "/tmp/filetochangeattributesof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);

            var originalAttributes = txFileSystem.File.GetAttributes(fileName);
            var modifiedAttributes = originalAttributes | FileAttributes.Compressed;

            using var transactionScope = new TransactionScope();

            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.File.SetAttributes(fileName, modifiedAttributes);

            transactionScope.Complete();

            Assert.Equal(modifiedAttributes, txFileSystem.File.GetAttributes(fileName));
        }

        [Fact]
        public void SetAttributesOperation_ExceptionThrown_AttributesUnchanged()
        {
            var fileName = "/tmp/filetochangeattributesof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);

            var originalAttributes = txFileSystem.File.GetAttributes(fileName);
            var modifiedAttributes = originalAttributes | FileAttributes.Compressed;

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();

                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.SetAttributes(fileName, modifiedAttributes);

                throw new Exception("Error occurred right after changing attributes");
            });

            Assert.NotEqual(modifiedAttributes, txFileSystem.File.GetAttributes(fileName));
            Assert.Equal(originalAttributes, txFileSystem.File.GetAttributes(fileName));
        }
    }
}
