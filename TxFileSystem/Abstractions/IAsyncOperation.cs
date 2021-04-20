namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

    internal interface IAsyncOperation
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
