namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
#if !NETSTANDARD2_0 && !NET461
    using System.Threading;
    using System.Threading.Tasks;
#endif

    internal sealed class ReadAllBytesOperation : FileOperation, IReturningOperation<byte[]>
#if !NETSTANDARD2_0 && !NET461
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

#if !NETSTANDARD2_0 && !NET461
        public Task<byte[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            return _file.TxFileSystem.FileSystem.File.ReadAllBytesAsync(_path, cancellationToken);
        }
#endif
    }
}
