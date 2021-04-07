namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class ReadAllLinesOperation : FileOperation, IReturningOperation<string[]>,
        IAsyncReturningOperation<string[]>
    {
        private readonly Encoding _encoding = null;

        public ReadAllLinesOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public ReadAllLinesOperation(ITxFile file, string path, Encoding encoding)
            : this(file, path)
        {
            _encoding = encoding;
        }

        public override OperationType OperationType => OperationType.Read;

        public string[] Execute()
        {
            Journalize(this);

            if (_encoding != null)
            {
                return _file.TxFileSystem.FileSystem.File.ReadAllLines(_path, _encoding);
            }

            return _file.TxFileSystem.FileSystem.File.ReadAllLines(_path);
        }

        public Task<string[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            if (_encoding != null)
            {
                return _file.TxFileSystem.FileSystem.File.ReadAllLinesAsync(_path, _encoding, cancellationToken);
            }

            return _file.TxFileSystem.FileSystem.File.ReadAllLinesAsync(_path, cancellationToken);
        }
    }
}
