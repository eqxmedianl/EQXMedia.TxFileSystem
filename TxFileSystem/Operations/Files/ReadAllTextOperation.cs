namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Text;
#if !NETSTANDARD2_0 && !NET461
    using System.Threading;
    using System.Threading.Tasks;
#endif

    internal sealed class ReadAllTextOperation : FileOperation, IReturningOperation<string>
#if !NETSTANDARD2_0 && !NET461
        , IAsyncReturningOperation<string>
#endif
    {
        private readonly Encoding _encoding = null;

        public ReadAllTextOperation(TxFile file, string path)
            : base(file, path)
        {
        }
        public ReadAllTextOperation(TxFile file, string path, Encoding encoding)
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
                return _file.TxFileSystem.FileSystem.File.ReadAllText(_path, _encoding);
            }

            return _file.TxFileSystem.FileSystem.File.ReadAllText(_path);
        }

#if !NETSTANDARD2_0 && !NET461
        public Task<string> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            if (_encoding != null)
            {
                return _file.TxFileSystem.FileSystem.File.ReadAllTextAsync(_path, _encoding, cancellationToken);
            }

            return _file.TxFileSystem.FileSystem.File.ReadAllTextAsync(_path, cancellationToken);
        }
#endif
    }
}
