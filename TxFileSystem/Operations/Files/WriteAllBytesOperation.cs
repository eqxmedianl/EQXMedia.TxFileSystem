namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class WriteAllBytesOperation : FileOperation, IExecutingOperation, IAsyncOperation
    {
        private readonly byte[] _bytes = null;

        public WriteAllBytesOperation(ITxFile file, string path, byte[] bytes)
            : base(file, path)
        {
            _bytes = bytes;
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            _file.FileSystem.File.WriteAllBytes(_path, _bytes);
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            return _file.FileSystem.File.WriteAllBytesAsync(_path, _bytes, cancellationToken);
        }
    }
}
