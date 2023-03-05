namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.TestingHelpers.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Reflection;
    using System.Transactions;
    using Xunit;

    public sealed class ReplaceOperation_Should
    {
        [Fact]
        public void ReplaceOperation_ProperlyExecuted()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            using (var transactionScope = new TransactionScope())
            {    
                txFileSystem.Directory.CreateDirectory("/tmp");

                var sourceTextStream = txFileSystem.File.CreateText("/tmp/sourcefile.txt");
                sourceTextStream.Write("Source file contents");
                sourceTextStream.Flush();
                sourceTextStream.Close();

                var destTextStream = txFileSystem.File.CreateText("/tmp/destfile.txt");
                destTextStream.Write("Dest file contents");
                destTextStream.Flush();
                destTextStream.Close();

                txFileSystem.File.Replace("/tmp/sourcefile.txt", "/tmp/destfile.txt", "/tmp/destbackupfile.txt");

                transactionScope.Complete();
            }

            Assert.False(txFileSystem.File.Exists("/tmp/sourcefile.txt"));
            Assert.True(txFileSystem.File.Exists("/tmp/destfile.txt"));
            Assert.True(txFileSystem.File.Exists("/tmp/destbackupfile.txt"));
            Assert.Equal("Source file contents", txFileSystem.File.ReadAllText("/tmp/destfile.txt"));
            Assert.Equal("Dest file contents", txFileSystem.File.ReadAllText("/tmp/destbackupfile.txt"));
        }

        [Fact]
        public void ReplaceOperation_ProperlyRolledBack()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            var sourceTextStream = txFileSystem.File.CreateText("/tmp/sourcefile.txt");
            sourceTextStream.Write("Source file contents");
            sourceTextStream.Flush();
            sourceTextStream.Close();

            var destTextStream = txFileSystem.File.CreateText("/tmp/destfile.txt");
            destTextStream.Write("Dest file contents");
            destTextStream.Flush();
            destTextStream.Close();

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.File.Replace("/tmp/sourcefile.txt", "/tmp/destfile.txt", "/tmp/destbackupfile.txt");

                    throw new Exception("Error occurred right after replacing file");
                }
            });

            Assert.True(txFileSystem.File.Exists("/tmp/sourcefile.txt"));
            Assert.True(txFileSystem.File.Exists("/tmp/destfile.txt"));
            Assert.False(txFileSystem.File.Exists("/tmp/destbackupfile.txt"));
            Assert.Equal("Source file contents", txFileSystem.File.ReadAllText("/tmp/sourcefile.txt"));
            Assert.Equal("Dest file contents", txFileSystem.File.ReadAllText("/tmp/destfile.txt"));
        }

        [Fact, FsFact]
        public void ReplaceOperation_IgnoreMetaDataErrors_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            fileSystemMock.Setup(f => f.File.Replace(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            fileSystemMock.Setup(f => f.File.Replace(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<bool>()));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.Replace("/tmp/sourcefile.txt", "/tmp/destfile.txt", "/tmp/destfilebackup.txt", true);

            fileSystemMock.Verify(f => f.File.Replace(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
            fileSystemMock.Verify(f => f.File.Replace(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<bool>()), Times.Once);
        }
    }
}
