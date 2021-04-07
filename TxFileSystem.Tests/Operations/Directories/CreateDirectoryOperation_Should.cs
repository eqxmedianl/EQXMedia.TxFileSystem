namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Journaling;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Transactions;
    using Xunit;

    public sealed class CreateDirectoryOperation_Should
    {
        [Fact]
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
                using var transactionScope = new TransactionScope();
                txFileSystem = new TxFileSystem(fileSystemMock.Object);
                txFileSystem.Directory.CreateDirectory("/var/www");
                txFileSystem.Directory.CreateDirectory("/var/failingdirectory");
                transactionScope.Complete();
            });

            Assert.True(((ITxFileSystem)txFileSystem).Journal.IsRolledBack);
            Assert.False(txFileSystem.Directory.Exists("/var/www"));
            Assert.False(txFileSystem.Directory.Exists("/var/failingdirectory"));
        }

        [Fact]
        public void CreateDirectoryOperation_ResultsInExists_ReturnsTrue()
        {
            TxFileSystem txFileSystem = null;
            
            void CreateDirectories()
            {
                var mockFileSystem = new MockFileSystem();
                using var transactionScope = new TransactionScope();
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.Directory.CreateDirectory("/var/www");
                txFileSystem.Directory.CreateDirectory("/var/nonfailingdirectory");
                transactionScope.Complete();
            }

            CreateDirectories();

            var txJournal = ((ITxFileSystem)txFileSystem).Journal;

            Assert.Equal(JournalState.Committed, txJournal.State);
            Assert.False(txJournal.IsRolledBack);
            Assert.True(txFileSystem.Directory.Exists("/var/www"));
            Assert.True(txFileSystem.Directory.Exists("/var/nonfailingdirectory"));
        }

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
