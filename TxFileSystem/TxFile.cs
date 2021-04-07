namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Operations.Files;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Security.AccessControl;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class TxFile : IFile, ITxFile
    {
        public TxFile(ITxFileSystem fileSystem)
        {
            ((ITxFile)this).TxFileSystem = fileSystem;
        }

        public IFileSystem FileSystem => ((ITxFile)this).TxFileSystem.FileSystem;

        ITxFileSystem ITxFile.TxFileSystem { get; set; }

        public void AppendAllLines(string path, IEnumerable<string> contents)
        {
            new AppendAllLinesOperation(this, path, contents).Execute();
        }

        public void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            new AppendAllLinesOperation(this, path, contents, encoding).Execute();
        }

        public Task AppendAllLinesAsync(string path, IEnumerable<string> contents,
            CancellationToken cancellationToken = default)
        {
            return new AppendAllLinesOperation(this, path, contents).ExecuteAsync(cancellationToken);
        }

        public Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new AppendAllLinesOperation(this, path, contents, encoding).ExecuteAsync(cancellationToken);
        }

        public void AppendAllText(string path, string contents)
        {
            new AppendAllTextOperation(this, path, contents).Execute();
        }

        public void AppendAllText(string path, string contents, Encoding encoding)
        {
            new AppendAllTextOperation(this, path, contents, encoding).Execute();
        }

        public Task AppendAllTextAsync(string path, string contents,
            CancellationToken cancellationToken = default)
        {
            return new AppendAllTextOperation(this, path, contents).ExecuteAsync(cancellationToken);
        }

        public Task AppendAllTextAsync(string path, string contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new AppendAllTextOperation(this, path, contents, encoding).ExecuteAsync(cancellationToken);
        }

        public StreamWriter AppendText(string path)
        {
            return new AppendTextOperation(this, path).Execute();
        }

        public void Copy(string sourceFileName, string destFileName)
        {
            new CopyOperation(this, sourceFileName, destFileName).Execute();
        }

        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            new CopyOperation(this, sourceFileName, destFileName, overwrite).Execute();
        }

        public Stream Create(string path)
        {
            return new CreateOperation(this, path).Execute();
        }

        public Stream Create(string path, int bufferSize)
        {
            return new CreateOperation(this, path, bufferSize).Execute();
        }

        public Stream Create(string path, int bufferSize, FileOptions options)
        {
            return new CreateOperation(this, path, bufferSize, options).Execute();
        }

        public StreamWriter CreateText(string path)
        {
            return new CreateTextOperation(this, path).Execute();
        }

        public void Decrypt(string path)
        {
            new DecryptOperation(this, path).Execute();
        }

        public void Delete(string path)
        {
            new DeleteOperation(this, path).Execute();
        }

        public void Encrypt(string path)
        {
            new EncryptOperation(this, path).Execute();
        }

        public bool Exists(string path)
        {
            return new ExistsOperation(this, path).Execute();
        }

        public FileSecurity GetAccessControl(string path)
        {
            return new GetAccessControlOperation(this, path).Execute();
        }

        public FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return new GetAccessControlOperation(this, path, includeSections).Execute();
        }

        public FileAttributes GetAttributes(string path)
        {
            return new GetAttributesOperation(this, path).Execute();
        }

        public DateTime GetCreationTime(string path)
        {
            return new GetCreationTimeOperation(this, path).Execute();
        }

        public DateTime GetCreationTimeUtc(string path)
        {
            return new GetCreationTimeOperation(this, path, asUtc: true).Execute();
        }

        public DateTime GetLastAccessTime(string path)
        {
            return new GetLastAccessTimeOperation(this, path).Execute();
        }

        public DateTime GetLastAccessTimeUtc(string path)
        {
            return new GetLastAccessTimeOperation(this, path, asUtc: true).Execute();
        }

        public DateTime GetLastWriteTime(string path)
        {
            return new GetLastWriteTimeOperation(this, path).Execute();
        }

        public DateTime GetLastWriteTimeUtc(string path)
        {
            return new GetLastWriteTimeOperation(this, path, asUtc: true).Execute();
        }

        public void Move(string sourceFileName, string destFileName)
        {
            new MoveOperation(this, sourceFileName, destFileName).Execute();
        }

        public Stream Open(string path, FileMode mode)
        {
            return new OpenOperation(this, path, mode).Execute();
        }

        public Stream Open(string path, FileMode mode, FileAccess access)
        {
            return new OpenOperation(this, path, mode, access).Execute();
        }

        public Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return new OpenOperation(this, path, mode, access, share).Execute();
        }

        public Stream OpenRead(string path)
        {
            return new OpenReadOperation(this, path).Execute();
        }

        public StreamReader OpenText(string path)
        {
            return new OpenTextOperation(this, path).Execute();
        }

        public Stream OpenWrite(string path)
        {
            return new OpenWriteOperation(this, path).Execute();
        }

        public byte[] ReadAllBytes(string path)
        {
            return new ReadAllBytesOperation(this, path).Execute();
        }

        public Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
        {
            return new ReadAllBytesOperation(this, path).ExecuteAsync(cancellationToken);
        }

        public string[] ReadAllLines(string path)
        {
            return new ReadAllLinesOperation(this, path).Execute();
        }

        public string[] ReadAllLines(string path, Encoding encoding)
        {
            return new ReadAllLinesOperation(this, path, encoding).Execute();
        }

        public Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default)
        {
            return new ReadAllLinesOperation(this, path).ExecuteAsync(cancellationToken);
        }

        public Task<string[]> ReadAllLinesAsync(string path, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new ReadAllLinesOperation(this, path, encoding).ExecuteAsync(cancellationToken);
        }

        public string ReadAllText(string path)
        {
            return new ReadAllTextOperation(this, path).Execute();
        }

        public string ReadAllText(string path, Encoding encoding)
        {
            return new ReadAllTextOperation(this, path, encoding).Execute();
        }

        public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
        {
            return new ReadAllTextOperation(this, path).ExecuteAsync(cancellationToken);
        }

        public Task<string> ReadAllTextAsync(string path, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new ReadAllTextOperation(this, path, encoding).ExecuteAsync(cancellationToken);
        }

        public IEnumerable<string> ReadLines(string path)
        {
            return new ReadLinesOperation(this, path).Execute();
        }

        public IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            return new ReadLinesOperation(this, path, encoding).Execute();
        }

        public void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            new ReplaceOperation(this, sourceFileName, destinationFileName, destinationBackupFileName)
                .Execute();
        }

        public void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName,
            bool ignoreMetadataErrors)
        {
            new ReplaceOperation(this, sourceFileName, destinationFileName, destinationBackupFileName,
                ignoreMetadataErrors)
                .Execute();
        }

        public void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            new SetAccessControlOperation(this, path, fileSecurity).Execute();
        }

        public void SetAttributes(string path, FileAttributes fileAttributes)
        {
            new SetAttributesOperation(this, path, fileAttributes).Execute();
        }

        public void SetCreationTime(string path, DateTime creationTime)
        {
            new SetCreationTimeOperation(this, path, creationTime).Execute();
        }

        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            new SetCreationTimeOperation(this, path, creationTimeUtc, asUtc: true).Execute();
        }

        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            new SetLastAccessTimeOperation(this, path, lastAccessTime).Execute();
        }

        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            new SetLastAccessTimeOperation(this, path, lastAccessTimeUtc, asUtc: true).Execute();
        }

        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            new SetLastWriteTimeOperation(this, path, lastWriteTime).Execute();
        }

        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            new SetLastWriteTimeOperation(this, path, lastWriteTimeUtc, asUtc: true).Execute();
        }

        public void WriteAllBytes(string path, byte[] bytes)
        {
            new WriteAllBytesOperation(this, path, bytes).Execute();
        }

        public Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
        {
            return new WriteAllBytesOperation(this, path, bytes).ExecuteAsync(cancellationToken);
        }

        public void WriteAllLines(string path, IEnumerable<string> contents)
        {
            new WriteAllLinesOperation(this, path, contents).Execute();
        }

        public void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            new WriteAllLinesOperation(this, path, contents, encoding).Execute();
        }

        public void WriteAllLines(string path, string[] contents)
        {
            new WriteAllLinesOperation(this, path, contents).Execute();
        }

        public void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            new WriteAllLinesOperation(this, path, contents, encoding).Execute();
        }

        public Task WriteAllLinesAsync(string path, IEnumerable<string> contents,
            CancellationToken cancellationToken = default)
        {
            return new WriteAllLinesOperation(this, path, contents).ExecuteAsync(cancellationToken);
        }

        public Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new WriteAllLinesOperation(this, path, contents, encoding).ExecuteAsync(cancellationToken);
        }

        public Task WriteAllLinesAsync(string path, string[] contents,
            CancellationToken cancellationToken = default)
        {
            return new WriteAllLinesOperation(this, path, contents).ExecuteAsync(cancellationToken);
        }

        public Task WriteAllLinesAsync(string path, string[] contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new WriteAllLinesOperation(this, path, contents, encoding).ExecuteAsync(cancellationToken);
        }

        public void WriteAllText(string path, string contents)
        {
            new WriteAllTextOperation(this, path, contents).Execute();
        }

        public void WriteAllText(string path, string contents, Encoding encoding)
        {
            new WriteAllTextOperation(this, path, contents, encoding).Execute();
        }

        public Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default)
        {
            return new WriteAllTextOperation(this, path, contents).ExecuteAsync(cancellationToken);
        }

        public Task WriteAllTextAsync(string path, string contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new WriteAllTextOperation(this, path, contents, encoding).ExecuteAsync(cancellationToken);
        }
    }
}