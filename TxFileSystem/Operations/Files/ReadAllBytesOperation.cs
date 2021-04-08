namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
#if !NETSTANDARD2_0
    using System.Threading;
    using System.Threading.Tasks;
#endif

    internal sealed class ReadAllBytesOperation : FileOperation, IReturningOperation<byte[]>
#if !NETSTANDARD2_0
        , IAsyncReturningOperation<byte[]>
#endif
    {
        public ReadAllBytesOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public override OperationType OperationType => OperationType.Read;

        public byte[] Execute()
        {
            Journalize(this);

            return _file.FileSystem.File.ReadAllBytes(_path);
        }

#if !NETSTANDARD2_0
        public Task<byte[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            return _file.FileSystem.File.ReadAllBytesAsync(_path, cancellationToken);
        }
#endif
    }
}
