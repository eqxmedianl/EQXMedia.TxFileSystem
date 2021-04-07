namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IAsyncOperation
    {
        public Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
