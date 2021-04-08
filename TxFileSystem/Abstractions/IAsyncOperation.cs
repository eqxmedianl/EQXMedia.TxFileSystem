namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IAsyncOperation
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
