namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class SetAttributesOperation_Should
    {
        [Fact]
#if NETCOREAPP3_1_OR_GREATER
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void SetAttributesOperation_AttributesChanged()
        {
            var fileName = "/tmp/filetochangeattributesof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);

            var originalAttributes = txFileSystem.File.GetAttributes(fileName);
            var modifiedAttributes = originalAttributes | FileAttributes.Compressed;

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.SetAttributes(fileName, modifiedAttributes);

                transactionScope.Complete();

                Assert.Equal(modifiedAttributes, txFileSystem.File.GetAttributes(fileName));
            }
        }

        [Fact]
#if NETCOREAPP3_1_OR_GREATER
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
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
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.File.SetAttributes(fileName, modifiedAttributes);

                    throw new Exception("Error occurred right after changing attributes");
                }
            });

            Assert.NotEqual(modifiedAttributes, txFileSystem.File.GetAttributes(fileName));
            Assert.Equal(originalAttributes, txFileSystem.File.GetAttributes(fileName));
        }
    }
}
