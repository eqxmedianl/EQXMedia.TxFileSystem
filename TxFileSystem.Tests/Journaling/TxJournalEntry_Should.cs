namespace EQXMedia.TxFileSystem.Tests.Journaling
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Journaling;
    using global::EQXMedia.TxFileSystem.Operations;
    using global::EQXMedia.TxFileSystem.Tests.Operations;
    using System.IO.Abstractions.TestingHelpers;
    using Xunit;

    public sealed class TxJournalEntry_Should
    {
        [Fact]
        public void TxJournalEntry_Operation_EqualsOperationPassed()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            var unitTestOperation = new UnitTestDirectoryOperation((ITxDirectory)txFileSystem.Directory,
                "/var/journaltestdir");

            var txJournalEntry = new TxJournalEntry(unitTestOperation);

            Assert.Equal(unitTestOperation, txJournalEntry.Operation);
            Assert.Equal(OperationType.Delete, txJournalEntry.Operation.OperationType);
        }
    }
}
