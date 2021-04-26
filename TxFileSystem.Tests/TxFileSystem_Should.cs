namespace EQXMedia.TxFileSystem.Tests
{
    using EQXMedia.TxFileSystem.Exceptions;
    using Xunit;

    public sealed class TxFileSystem_Should
    {
        [Fact]
        public void TxFileSystem_TxFileSystemPassed_ThrowsUnsupportedFileSystemImplementationException()
        {
            Assert.Throws<UnsupportedFileSystemImplementationException>(() =>
            {
                new TxFileSystem(new TxFileSystem());
            });
        }
    }
}
