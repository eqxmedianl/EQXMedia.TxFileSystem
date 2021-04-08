﻿namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Text;
#if !NETSTANDARD2_0
    using System.Threading;
    using System.Threading.Tasks;
#endif

    internal sealed class AppendAllTextOperation : FileOperation, IExecutingOperation
#if !NETSTANDARD2_0
        , IAsyncOperation
#endif
    {
        private readonly string _contents = null;
        private readonly Encoding _encoding = null;

        public AppendAllTextOperation(ITxFile file, string path, string contents)
            : base(file, path)
        {
            _contents = contents;
        }

        public AppendAllTextOperation(ITxFile file, string path, string contents, Encoding encoding)
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
                _file.FileSystem.File.AppendAllText(_path, _contents);
            }
            else
            {
                _file.FileSystem.File.AppendAllText(_path, _contents, _encoding);
            }
        }

#if !NETSTANDARD2_0
        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Journalize(this);

            if (_encoding == null)
            {
                return _file.FileSystem.File.AppendAllTextAsync(_path, _contents, cancellationToken);
            }
            else
            {
                return _file.FileSystem.File.AppendAllTextAsync(_path, _contents, _encoding, cancellationToken);
            }
        }
#endif
    }
}
