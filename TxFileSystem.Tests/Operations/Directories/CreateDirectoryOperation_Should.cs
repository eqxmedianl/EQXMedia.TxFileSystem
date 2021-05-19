namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Journaling;
    using Moq;
    using System;
#if SUPPRESS_SIMPLE_USING
    using System.Diagnostics.CodeAnalysis;
#endif
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Transactions;
    using Xunit;

#if NET5_0
    using System.Runtime.Versioning;
#endif

    public sealed class CreateDirectoryOperation_Should
    {
        [Fact]
#if SUPPRESS_SIMPLE_USING
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void CreateDirectoryOperation_Fails_ResultsInExists_ReturnsFalse()
        {
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(f => f.Directory.CreateDirectory(
                    It.Is<string>(s => s == "/var/failingdirectory"))
                )
                .Throws(new Exception("Failed to create failing directory"));

            TxFileSystem txFileSystem = null;

            Assert.Throws<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(fileSystemMock.Object);
                    txFileSystem.Directory.CreateDirectory("/var/www");
                    txFileSystem.Directory.CreateDirectory("/var/failingdirectory");
                    transactionScope.Complete();
                }
            });

            Assert.True(txFileSystem.Journal.IsRolledBack);
            Assert.False(txFileSystem.Directory.Exists("/var/www"));
            Assert.False(txFileSystem.Directory.Exists("/var/failingdirectory"));
        }

        [Fact]
#if SUPPRESS_SIMPLE_USING
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void CreateDirectoryOperation_ResultsInExists_ReturnsTrue()
        {
            TxFileSystem txFileSystem = null;

            void CreateDirectories()
            {
                #region CodeExample_CreateDirectory

                var mockFileSystem = new MockFileSystem();
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.Directory.CreateDirectory("/var/www");
                    txFileSystem.Directory.CreateDirectory("/var/nonfailingdirectory");
                    transactionScope.Complete();
                }

                #endregion
            }

            CreateDirectories();

            var txJournal = txFileSystem.Journal;

            Assert.Equal(JournalState.Committed, txJournal.State);
            Assert.False(txJournal.IsRolledBack);
            Assert.True(txFileSystem.Directory.Exists("/var/www"));
            Assert.True(txFileSystem.Directory.Exists("/var/nonfailingdirectory"));
        }

        [Fact]
#if SUPPRESS_SIMPLE_USING
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void CreateDirectoryOperation_OnExistingDirectory_ExceptionThrown_ResultsInExists_ReturnsTrue()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/alreadyexistingdirectory");

            Assert.True(txFileSystem.Directory.Exists("/var/alreadyexistingdirectory"));

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.Directory.CreateDirectory("/var/alreadyexistingdirectory");

                    throw new Exception("Error occurs after calling CreateDirectory on existing directory");
                }
            });

            Assert.Equal(JournalState.RolledBack, txFileSystem.Journal.State);
            Assert.True(txFileSystem.Journal.IsRolledBack);
            Assert.True(txFileSystem.Directory.Exists("/var/alreadyexistingdirectory"));
        }

#if SUPPORTED_OS_PLATFORM
        [SupportedOSPlatform("windows")]
#endif
        [Fact]
        public void CreateDirectoryOperation_WithDirectorySecurity_ReturnsSameFileAccessRule()
        {
            var fileSystemAccessRule = new FileSystemAccessRule(
                Environment.UserName, FileSystemRights.Read, AccessControlType.Allow);

            var directorySecurity = new DirectorySecurity();
            directorySecurity.AddAccessRule(fileSystemAccessRule);

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            var directoryInfo = txFileSystem.Directory.CreateDirectory("/temp/dirwithaccessrestriction",
                directorySecurity);
            var authorizationRule = directoryInfo.GetAccessControl()
                .GetAccessRules(true, true, typeof(NTAccount))[0] as FileSystemAccessRule;

            Assert.IsType<FileSystemAccessRule>(authorizationRule);
            Assert.True(authorizationRule.FileSystemRights.HasFlag(FileSystemRights.Read));

            var identityUserName = authorizationRule.IdentityReference.Value;
            if (identityUserName.Contains(@"\"))
            {
                var startPos = identityUserName.LastIndexOf(@"\") + 1;
                identityUserName = identityUserName.Substring(startPos, identityUserName.Length - startPos);
            }

            Assert.Equal(Environment.UserName, identityUserName);
        }

#if SUPPORTED_OS_PLATFORM
        [SupportedOSPlatform("windows")]
#endif
        [Fact]
        public void CreateDirectoryOperation_WithoutDirectorySecurity_ReturnsEmptyAuthorizationRuleCollection()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            var directoryInfo = txFileSystem.Directory.CreateDirectory("/temp/dirwithoutsecurity");
            var authorizationRuleCollection = directoryInfo.GetAccessControl()
                .GetAccessRules(true, true, typeof(NTAccount));

            Assert.Empty(authorizationRuleCollection);
        }
    }
}
