namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
#if ASYNC_IO
    using System.Threading;
    using System.Threading.Tasks;
#endif

    internal sealed class ReadAllBytesOperation : FileOperation, IReturningOperation<byte[]>
#if ASYNC_IO
        , IAsyncReturningOperation<byte[]>
#endif
    {
        public ReadAllBytesOperation(TxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Read;

        public byte[] Execute()
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.ReadAllBytes(_path);
        }

#if ASYNC_IO
        public Task<byte[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.ReadAllBytesAsync(_path, cancellationToken);
        }
#endif
    }
}
