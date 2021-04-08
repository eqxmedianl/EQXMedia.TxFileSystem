namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class ReadAllTextOperation : FileOperation, IReturningOperation<string>,
        IAsyncReturningOperation<string>
    {
        private readonly Encoding _encoding = null;

        public ReadAllTextOperation(ITxFile file, string path)
            : base(file, path)
        {
        }
        public ReadAllTextOperation(ITxFile file, string path, Encoding encoding)
            : this(file, path)
        {
            _encoding = encoding;
        }

        public override OperationType OperationType => OperationType.Read;

        public string Execute()
        {
            Journalize(this);

            if (_encoding != null)
            {
                return _file.FileSystem.File.ReadAllText(_path, _encoding);
            }

            return _file.FileSystem.File.ReadAllText(_path);
        }

        public Task<string> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            if (_encoding != null)
            {
                return _file.FileSystem.File.ReadAllTextAsync(_path, _encoding, cancellationToken);
            }

            return _file.FileSystem.File.ReadAllTextAsync(_path, cancellationToken);
        }
    }
}
