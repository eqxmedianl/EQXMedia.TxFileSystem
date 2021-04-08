namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Collections.Generic;
    using System.Text;
#if !NETSTANDARD2_0
    using System.Threading;
    using System.Threading.Tasks;
#endif

    internal sealed class AppendAllLinesOperation : FileOperation, IExecutingOperation
#if !NETSTANDARD2_0
        , IAsyncOperation
#endif
    {
        private readonly IEnumerable<string> _contents = null;
        private readonly Encoding _encoding = null;

        public AppendAllLinesOperation(ITxFile file, string path, IEnumerable<string> contents)
            : base(file, path)
        {
            _contents = contents;
        }

        public AppendAllLinesOperation(ITxFile file, string path, IEnumerable<string> contents, Encoding encoding)
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
                _file.FileSystem.File.AppendAllLines(_path, _contents);
            }
            else
            {
                _file.FileSystem.File.AppendAllLines(_path, _contents, _encoding);
            }
        }

#if !NETSTANDARD2_0
        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            if (_encoding == null)
            {
                return _file.FileSystem.File.AppendAllLinesAsync(_path, _contents, cancellationToken);
            }
            else
            {
                return _file.FileSystem.File.AppendAllLinesAsync(_path, _contents, _encoding, cancellationToken);
            }
        }
#endif
    }
}