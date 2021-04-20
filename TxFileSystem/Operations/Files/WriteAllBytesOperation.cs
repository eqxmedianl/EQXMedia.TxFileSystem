namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
#if !NETSTANDARD2_0 && !NET461
    using System.Threading;
    using System.Threading.Tasks;
#endif

    internal sealed class WriteAllBytesOperation : FileOperation, IExecutingOperation
#if !NETSTANDARD2_0 && !NET461
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

#if !NETSTANDARD2_0 && !NET461
        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.WriteAllBytesAsync(_path, _bytes, cancellationToken);
        }
#endif
    }
}
