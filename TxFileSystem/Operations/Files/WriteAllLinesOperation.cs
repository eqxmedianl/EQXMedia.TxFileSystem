namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Collections.Generic;
    using System.Text;
#if !NETSTANDARD2_0
    using System.Threading;
    using System.Threading.Tasks;
#endif

    internal sealed class WriteAllLinesOperation : FileOperation, IExecutingOperation
#if !NETSTANDARD2_0
        , IAsyncOperation
#endif
    {
        private readonly IEnumerable<string> _contentsEnumerable = null;
        private readonly string[] _contentsArray = null;
        private readonly Encoding _encoding = null;

        public WriteAllLinesOperation(ITxFile file, string path, IEnumerable<string> contents)
            : base(file, path)
        {
            _contentsEnumerable = contents;
        }

        public WriteAllLinesOperation(ITxFile file, string path, IEnumerable<string> contents, Encoding encoding)
            : this(file, path, contents)
        {
            _encoding = encoding;
        }

        public WriteAllLinesOperation(ITxFile file, string path, string[] contents)
            : base(file, path)
        {
            _contentsArray = contents;
        }

        public WriteAllLinesOperation(ITxFile file, string path, string[] contents, Encoding encoding)
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
                    _file.FileSystem.File.WriteAllLines(_path, _contentsEnumerable);
                }
                else
                {
                    _file.FileSystem.File.WriteAllLines(_path, _contentsEnumerable, _encoding);
                }
            }
            else
            {
                if (_encoding == null)
                {
                    _file.FileSystem.File.WriteAllLines(_path, _contentsArray);
                }
                else
                {
                    _file.FileSystem.File.WriteAllLines(_path, _contentsArray, _encoding);
                }
            }
        }

#if !NETSTANDARD2_0
        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            if (_contentsEnumerable != null)
            {
                if (_encoding == null)
                {
                    return _file.FileSystem.File.WriteAllLinesAsync(_path, _contentsEnumerable,
                        cancellationToken);
                }
                else
                {
                    return _file.FileSystem.File.WriteAllLinesAsync(_path, _contentsEnumerable, _encoding,
                        cancellationToken);
                }
            }
            else
            {
                if (_encoding == null)
                {
                    return _file.FileSystem.File.WriteAllLinesAsync(_path, _contentsArray,
                        cancellationToken);
                }
                else
                {
                    return _file.FileSystem.File.WriteAllLinesAsync(_path, _contentsArray, _encoding,
                        cancellationToken);
                }
            }
        }
#endif
    }
}
