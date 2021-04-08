namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

    internal interface IAsyncReturningOperation<TReturn> : IOperation
    {
        Task<TReturn> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
