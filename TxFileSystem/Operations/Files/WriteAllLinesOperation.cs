namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Collections.Generic;
    using System.Text;
#if ASYNC_IO
    using System.Threading;
    using System.Threading.Tasks;
#endif

    internal sealed class WriteAllLinesOperation : FileOperation, IExecutingOperation
#if ASYNC_IO
        , IAsyncOperation
#endif
    {
        private readonly IEnumerable<string> _contentsEnumerable = null;
        private readonly string[] _contentsArray = null;
        private readonly Encoding _encoding = null;

        public WriteAllLinesOperation(TxFile file, string path, IEnumerable<string> contents)
            : base(file, path)
        {
            _contentsEnumerable = contents;
        }

        public WriteAllLinesOperation(TxFile file, string path, IEnumerable<string> contents, Encoding encoding)
            : this(file, path, contents)
        {
            _encoding = encoding;
        }

        public WriteAllLinesOperation(TxFile file, string path, string[] contents)
            : base(file, path)
        {
            _contentsArray = contents;
        }

        public WriteAllLinesOperation(TxFile file, string path, string[] contents, Encoding encoding)
            : this(file, path, contents)
        {
            _encoding = encoding;
        }

        public override OperationType OperationType => OperationType.Modify;

        public void Execute()
        {
            Journalize(this);

            if (_contentsEnumerable != null)
            {
                if (_encoding == null)
                {
                    _file.TxFileSystem.FileSystem.File.WriteAllLines(_path, _contentsEnumerable);
                }
                else
                {
                    _file.TxFileSystem.FileSystem.File.WriteAllLines(_path, _contentsEnumerable, _encoding);
                }
            }
            else
            {
                if (_encoding == null)
                {
                    _file.TxFileSystem.FileSystem.File.WriteAllLines(_path, _contentsArray);
                }
                else
                {
                    _file.TxFileSystem.FileSystem.File.WriteAllLines(_path, _contentsArray, _encoding);
                }
            }
        }

#if ASYNC_IO
        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            if (_contentsEnumerable != null)
            {
                if (_encoding == null)
                {
                    return _file.TxFileSystem.FileSystem.File.WriteAllLinesAsync(_path, _contentsEnumerable,
                        cancellationToken);
                }
                else
                {
                    return _file.TxFileSystem.FileSystem.File.WriteAllLinesAsync(_path, _contentsEnumerable, _encoding,
                        cancellationToken);
                }
            }
            else
            {
                if (_encoding == null)
                {
                    return _file.TxFileSystem.FileSystem.File.WriteAllLinesAsync(_path, _contentsArray,
                        cancellationToken);
                }
                else
                {
                    return _file.TxFileSystem.FileSystem.File.WriteAllLinesAsync(_path, _contentsArray, _encoding,
                        cancellationToken);
                }
            }
        }
#endif
    }
}
