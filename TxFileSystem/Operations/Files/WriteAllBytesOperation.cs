namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
#if ASYNC_IO
    using System.Threading;
    using System.Threading.Tasks;
#endif

    internal sealed class WriteAllBytesOperation : FileOperation, IExecutingOperation
#if ASYNC_IO
        , IAsyncOperation
#endif
    {
        private readonly byte[] _bytes = null;

        public WriteAllBytesOperation(TxFile file, string path, byte[] bytes)
            : base(file, path)
        {
            _bytes = bytes;
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            _file.TxFileSystem.FileSystem.File.WriteAllBytes(_path, _bytes);
        }

#if ASYNC_IO
        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.WriteAllBytesAsync(_path, _bytes, cancellationToken);
        }
#endif
    }
}
