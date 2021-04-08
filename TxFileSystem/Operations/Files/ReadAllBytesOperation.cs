namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class ReadAllBytesOperation : FileOperation, IReturningOperation<byte[]>, IAsyncReturningOperation<byte[]>
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

        public Task<byte[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            return _file.FileSystem.File.ReadAllBytesAsync(_path, cancellationToken);
        }
    }
}
