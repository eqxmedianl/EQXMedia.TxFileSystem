namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Security.AccessControl;
    using System.Transactions;
    using Xunit;

#if NET5_0
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
#endif
    public sealed class SetAccessControlOperation_Should
    {
        [Fact]
        public void SetAccessControlOperation_AccessControlChanged()
        {
            var dirName = "/tmp/dirtochangeaccesscontrolof";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory(dirName);

            var originalAccessControl = txFileSystem.Directory.GetAccessControl(dirName);
            var modifiedAccessControl = new DirectorySecurity();

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.Directory.SetAccessControl(dirName, modifiedAccessControl);

                transactionScope.Complete();
            }

            Assert.NotEqual(originalAccessControl, txFileSystem.Directory.GetAccessControl(dirName));
            Assert.Equal(modifiedAccessControl, txFileSystem.Directory.GetAccessControl(dirName));
        }

        [Fact]
        public void SetAccessControlOperation_ExceptionThrown_AccessControlUnchanged()
        {
            var dirName = "/tmp/dirtochangeaccesscontrolof";
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory(dirName);

            var originalAccessControl = txFileSystem.Directory.GetAccessControl(dirName);
            var modifiedAccessControl = new DirectorySecurity();

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.Directory.SetAccessControl(dirName, modifiedAccessControl);

                    throw new Exception("Error occurred right after changing accesscontrol");
                }
            });

            Assert.NotEqual(modifiedAccessControl, txFileSystem.Directory.GetAccessControl(dirName));
            Assert.Equal(originalAccessControl, txFileSystem.Directory.GetAccessControl(dirName));
        }
    }
}
