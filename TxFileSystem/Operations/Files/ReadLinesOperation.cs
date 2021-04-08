namespace EQXMedia.TxFileSystem.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.Collections.Generic;
    using System.Text;

    internal sealed class ReadLinesOperation : FileOperation, IReturningOperation<IEnumerable<string>>
    {
        private readonly Encoding _encoding = null;

        public ReadLinesOperation(ITxFile file, string path)
            : base(file, path)
        {
        }

        public ReadLinesOperation(ITxFile file, string path, Encoding encoding)
            : this(file, path)
        {
            _encoding = encoding;
        }

        public override OperationType OperationType => OperationType.Read;

        public IEnumerable<string> Execute()
        {
            Journalize(this);

            if (_encoding != null)
            {
                return _file.FileSystem.File.ReadLines(_path, _encoding);
            }

            return _file.FileSystem.File.ReadLines(_path);
        }
    }
}
