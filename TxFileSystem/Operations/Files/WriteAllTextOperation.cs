namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class WriteAllTextOperation : FileOperation, IExecutingOperation, IAsyncOperation
    {
        private readonly string _contents = null;
        private readonly Encoding _encoding = null;

        public WriteAllTextOperation(ITxFile file, string path, string contents)
            : base(file, path)
        {
            _contents = contents;
        }

        public WriteAllTextOperation(ITxFile file, string path, string contents, Encoding encoding)
            : this(file, path, contents)
        {
            _encoding = encoding;
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            if (_encoding == null)
            {
                _file.TxFileSystem.FileSystem.File.WriteAllText(_path, _contents);
            }
            else
            {
                _file.TxFileSystem.FileSystem.File.WriteAllText(_path, _contents, _encoding);
            }
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            if (_encoding == null)
            {
                return _file.TxFileSystem.FileSystem.File.WriteAllTextAsync(_path, _contents, cancellationToken);
            }
            else
            {
                return _file.TxFileSystem.FileSystem.File.WriteAllTextAsync(_path, _contents, _encoding, cancellationToken);
            }
        }
    }
}
