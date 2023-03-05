namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.TestingHelpers.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using System.Transactions;
    using Xunit;

    public sealed class EncryptOperation_Should
    {
        [Fact, FsFact]
        public void EncryptOperation_Encrypt_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.Decrypt(It.Is<string>(s => s == "/tmp/filetoencrypt.txt")));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.Encrypt("/tmp/filetoencrypt.txt");

            fileSystemMock.Verify(f => f.File.Encrypt(It.Is<string>(s => s == "/tmp/filetoencrypt.txt")), Times.Once);
            fileSystemMock.Verify(f => f.File.Decrypt(It.IsAny<string>()), Times.Never);
        }

        [Fact, FsFact]
        public void EncryptOperation_ExceptionThrown_Decrypt_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.Decrypt(It.Is<string>(s => s == "/tmp/filetoencrypt.txt")));

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    var txFileSystem = new TxFileSystem(fileSystemMock.Object);
                    txFileSystem.File.Encrypt("/tmp/filetoencrypt.txt");

                    throw new Exception("Error occurred right after decrypting");
                }
            });

            fileSystemMock.Verify(f => f.File.Encrypt(It.Is<string>(s => s == "/tmp/filetoencrypt.txt")), Times.Once);
            fileSystemMock.Verify(f => f.File.Decrypt(It.Is<string>(s => s == "/tmp/filetoencrypt.txt")), Times.Once);
        }
    }
}
