namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Collections.Generic;
    using System.Text;
#if !NETSTANDARD2_0 && !NET461
    using System.Threading;
    using System.Threading.Tasks;
#endif

    internal sealed class AppendAllLinesOperation : FileOperation, IExecutingOperation
#if !NETSTANDARD2_0 && !NET461
        , IAsyncOperation
#endif
    {
        private readonly IEnumerable<string> _contents = null;
        private readonly Encoding _encoding = null;

        public AppendAllLinesOperation(TxFile file, string path, IEnumerable<string> contents)
            : base(file, path)
        {
            _contents = contents;
        }

        public AppendAllLinesOperation(TxFile file, string path, IEnumerable<string> contents, Encoding encoding)
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
                _file.TxFileSystem.FileSystem.File.AppendAllLines(_path, _contents);
            }
            else
            {
                _file.TxFileSystem.FileSystem.File.AppendAllLines(_path, _contents, _encoding);
            }
        }

#if !NETSTANDARD2_0 && !NET461
        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            if (_encoding == null)
            {
                return _file.TxFileSystem.FileSystem.File.AppendAllLinesAsync(_path, _contents, cancellationToken);
            }
            else
            {
                return _file.TxFileSystem.FileSystem.File.AppendAllLinesAsync(_path, _contents, _encoding, cancellationToken);
            }
        }
#endif
    }
}