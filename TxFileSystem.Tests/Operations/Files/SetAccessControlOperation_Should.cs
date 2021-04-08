namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Security.AccessControl;
    using System.Transactions;
    using Xunit;

#if NET5_0
    using System.Runtime.Versioning;
#endif

    public sealed class SetAccessControlOperation_Should
    {
#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        [Fact]
        public void SetAccessControlOperation_AccessControlChanged()
        {
            var fileName = "/tmp/filetochangeaccesscontrolof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);

            var originalAccessControl = txFileSystem.File.GetAccessControl(fileName);
            var modifiedAccessControl = new FileSecurity();

            using var transactionScope = new TransactionScope();

            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.File.SetAccessControl(fileName, modifiedAccessControl);

            transactionScope.Complete();

            Assert.NotEqual(originalAccessControl, txFileSystem.File.GetAccessControl(fileName));
            Assert.Equal(modifiedAccessControl, txFileSystem.File.GetAccessControl(fileName));
        }

#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        [Fact]
        public void SetAccessControlOperation_ExceptionThrown_AccessControlUnchanged()
        {
            var fileName = "/tmp/filetochangeaccesscontrolof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);

            var originalAccessControl = txFileSystem.File.GetAccessControl(fileName);
            var modifiedAccessControl = new FileSecurity();

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();

                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.SetAccessControl(fileName, modifiedAccessControl);

                throw new Exception("Error occurred right after changing accesscontrol");
            });

            Assert.NotEqual(modifiedAccessControl, txFileSystem.File.GetAccessControl(fileName));
            Assert.Equal(originalAccessControl, txFileSystem.File.GetAccessControl(fileName));
        }
    }
}
