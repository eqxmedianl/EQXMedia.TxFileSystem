namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.TestingHelpers.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Transactions;
    using Xunit;

    public sealed class DecryptOperation_Should
    {
        [Fact, FsFact]
        public void DecryptOperation_Decrypt_CalledOnce()
        {
            var fileName = "/tmp/filetodecrypt.txt";

            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.Decrypt(It.Is<string>(s => s == fileName)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.Decrypt(fileName);

            fileSystemMock.Verify(f => f.File.Encrypt(It.IsAny<string>()), Times.Never);
            fileSystemMock.Verify(f => f.File.Decrypt(It.Is<string>(s => s == fileName)), Times.Once);
        }

        [Fact, FsFact]
        public void DecryptOperation_ExceptionThrown_Encrypt_CalledOnce()
        {
            var fileName = "/tmp/filetodecrypt.txt";

            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.Decrypt(It.Is<string>(s => s == fileName)));

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    var txFileSystem = new TxFileSystem(fileSystemMock.Object);
                    txFileSystem.File.Decrypt(fileName);

                    throw new Exception("Error occurred right after decrypting");
                }
            });

            fileSystemMock.Verify(f => f.File.Decrypt(It.Is<string>(s => s == fileName)), Times.Once);
            fileSystemMock.Verify(f => f.File.Encrypt(It.Is<string>(s => s == fileName)), Times.Once);
        }
    }
}
