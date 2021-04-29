namespace EQXMedia.TxFileSystem.Tests.Journaling
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Journaling;
    using global::EQXMedia.TxFileSystem.TestingHelpers.Attributes;
    using global::EQXMedia.TxFileSystem.Tests.Operations;
    using System;
    using System.IO.Abstractions;
    using System.Reflection;
    using Xunit;

    public sealed class TxJournalEntryCollection_Should
    {
        [Fact]
        public void TxJournalEntryCollection_IsReadOnly_ReturnsFalse()
        {
            var journalEntries = new TxJournalEntryCollection();

            Assert.False(journalEntries.IsReadOnly);
        }

        [Fact, FsFact]
        public void TxJournalEntryCollection_Remove_ThrowsNotImplementedException()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var path = "/tmp/dummydirectory";
            var unitTestOperation = new UnitTestDirectoryOperation(txFileSystem.Directory, path);
            var journalEntry = new TxJournalEntry(unitTestOperation);
            var journalEntries = new TxJournalEntryCollection
            {
                journalEntry
            };

            Assert.Throws<NotImplementedException>(() =>
            {
                journalEntries.Remove(journalEntry);
            });
        }
    }
}
