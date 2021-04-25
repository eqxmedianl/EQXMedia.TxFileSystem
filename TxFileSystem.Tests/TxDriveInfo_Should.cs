namespace EQXMedia.TxFileSystem.Tests
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Reflection;
    using System.Transactions;
    using Xunit;

    public sealed class TxDriveInfo_Should
    {
        [Fact]
#if NETCOREAPP3_1_OR_GREATER
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void TxDriveInfo_ReturnsDriveInfo()
        {
            var mockFileSystem = new MockFileSystem();

            using (var transactionScope = new TransactionScope())
            {
                var txFileSystem = new TxFileSystem(mockFileSystem);
                var logicalDrives = txFileSystem.Directory.GetLogicalDrives();
                var driveInfo = txFileSystem.DriveInfo.FromDriveName(logicalDrives[0]);

                transactionScope.Complete();

                Assert.IsType<MockDriveInfo>(driveInfo);
            }
        }

        [Fact, FsFact]
        public void TxDriveInfo_FromDriveName_CalledOnce_ReturnsDriveInfo()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var driveName = @"C:\";
            var driveLabel = "MOCK_DRIVE";

            var driveInfoMock = new Mock<IDriveInfo>();
            driveInfoMock.SetupGet(d => d.VolumeLabel)
                .Returns(driveLabel);

            fileSystemMock.Setup(f => f.DriveInfo.FromDriveName(It.Is<string>((s) => s == driveName)))
                .Returns(driveInfoMock.Object);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var driveInfoReturned = txFileSystem.DriveInfo.FromDriveName(driveName);

            fileSystemMock.Verify(f => f.DriveInfo.FromDriveName(It.Is<string>((s) => s == driveName)),
                Times.Once);

            Assert.IsAssignableFrom<IDriveInfo>(driveInfoReturned);
            Assert.Equal(driveLabel, driveInfoReturned.VolumeLabel);
        }

        [Fact]
#if NETCOREAPP3_1_OR_GREATER
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void TxDriveInfo_ReturnsDrives()
        {
            var mockFileSystem = new MockFileSystem();

            using (var transactionScope = new TransactionScope())
            {
                var txFileSystem = new TxFileSystem(mockFileSystem);
                var drives = txFileSystem.DriveInfo.GetDrives();

                transactionScope.Complete();

                Assert.IsType<DriveInfoBase[]>(drives);
            }
        }

        [Fact, FsFact]
        public void TxDriveInfo_GetDrives_CalledOnce_ReturnsDriveInfoArray()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var drives = new string[] { @"C:\", @"D:\", @"E:\" };

            var cDriveInfoMock = new Mock<IDriveInfo>();
            cDriveInfoMock.SetupGet(d => d.VolumeLabel)
                .Returns(drives[0]);
            var dDriveInfoMock = new Mock<IDriveInfo>();
            dDriveInfoMock.SetupGet(d => d.VolumeLabel)
                .Returns(drives[1]);
            var eDriveInfoMock = new Mock<IDriveInfo>();
            eDriveInfoMock.SetupGet(d => d.VolumeLabel)
                .Returns(drives[2]);

            var drivesInfos = new IDriveInfo[] { cDriveInfoMock.Object, dDriveInfoMock.Object, eDriveInfoMock.Object };

            fileSystemMock.Setup(f => f.DriveInfo.GetDrives())
                .Returns(drivesInfos);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var driveInfosReturned = txFileSystem.DriveInfo.GetDrives();

            fileSystemMock.Verify(f => f.DriveInfo.GetDrives(), Times.Once);

            Assert.IsAssignableFrom<IDriveInfo[]>(driveInfosReturned);
            Assert.Equal(drivesInfos, driveInfosReturned);
        }

        [Fact]
#if NETCOREAPP3_1_OR_GREATER
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void TxDriveInfo_NotAddedToJournal()
        {
            var mockFileSystem = new MockFileSystem();

            using (var transactionScope = new TransactionScope())
            {
                var txFileSystem = new TxFileSystem(mockFileSystem);
                var logicalDrives = txFileSystem.Directory.GetLogicalDrives();
                txFileSystem.DriveInfo.FromDriveName(logicalDrives[0]);
                txFileSystem.DriveInfo.GetDrives();

                Assert.Empty(txFileSystem.Journal._txJournalEntries);
            }
        }
    }
}
