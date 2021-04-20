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
#if !NETSTANDARD2_0
    using System.Threading;
    using System.Threading.Tasks;
#endif

#if NET5_0
    using System.Runtime.Versioning;
#endif

    /// <summary>
    ///   Transactional file exposes methods for the creation, copying, deletion, moving, and opening of a single file,
    ///   aids in the creation of <see cref="System.IO.FileStream" /> objects and so on.
    ///   
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="true"]/summary/*' />
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     <c>TxFile</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.File" />. It can't 
    ///     be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </para>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="true"]/remarks/*' />
    /// </remarks>
    [Serializable]
    public sealed class TxFile : IFile
    {
        internal TxFile(TxFileSystem fileSystem)
        {
            this.TxFileSystem = fileSystem;
        }

        /// <summary>
        ///   Returns the <see cref="EQXMedia.TxFileSystem.TxFileSystem" />, which actually is an 
        ///   implementation of <c>System.IO.Abstractions.IFileSystem</c> itself too.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This property is exposed by the <c>System.IO.Abstractions.IFile</c> interface. The way it is 
        ///     implemented in this library, ensures that all operations performed through this property,
        ///     are transactional too. Whenever required.
        ///   </para>
        ///   <para>
        ///     This is useful for implementing extension methods.
        ///   </para>
        /// </remarks>
        public IFileSystem FileSystem => this.TxFileSystem;

        /// <summary>
        ///   Returns the <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> this <see 
        ///     cref="EQXMedia.TxFileSystem.TxFile" /> instance belongs to. Thus not the actual file system 
        ///     being wrapped.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///      Use <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileSystem" /> to perform operations on the 
        ///      wrapped file system.
        ///   </para>
        /// </remarks>
        internal TxFileSystem TxFileSystem { get; set; }

        /// <inheritdoc cref="System.IO.File.AppendAllLines(string, IEnumerable{string})"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void AppendAllLines(string path, IEnumerable<string> contents)
        {
            new AppendAllLinesOperation(this, path, contents).Execute();
        }

        /// <inheritdoc cref="System.IO.File.AppendAllLines(string, IEnumerable{string}, Encoding)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            new AppendAllLinesOperation(this, path, contents, encoding).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        /// <inheritdoc cref="System.IO.File.AppendAllLinesAsync(string, IEnumerable{string}, CancellationToken)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Task AppendAllLinesAsync(string path, IEnumerable<string> contents,
            CancellationToken cancellationToken = default)
        {
            return new AppendAllLinesOperation(this, path, contents).ExecuteAsync(cancellationToken);
        }

        /// <inheritdoc cref="System.IO.File.AppendAllLinesAsync(string, IEnumerable{string}, Encoding, CancellationToken)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new AppendAllLinesOperation(this, path, contents, encoding).ExecuteAsync(cancellationToken);
        }
#endif

        /// <inheritdoc cref="System.IO.File.AppendAllText(string, string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void AppendAllText(string path, string contents)
        {
            new AppendAllTextOperation(this, path, contents).Execute();
        }

        /// <inheritdoc cref="System.IO.File.AppendAllText(string, string, Encoding)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void AppendAllText(string path, string contents, Encoding encoding)
        {
            new AppendAllTextOperation(this, path, contents, encoding).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        /// <inheritdoc cref="System.IO.File.AppendAllTextAsync(string, string?, CancellationToken)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Task AppendAllTextAsync(string path, string contents,
            CancellationToken cancellationToken = default)
        {
            return new AppendAllTextOperation(this, path, contents).ExecuteAsync(cancellationToken);
        }

        /// <inheritdoc cref="System.IO.File.AppendAllTextAsync(string, string?, Encoding, CancellationToken)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Task AppendAllTextAsync(string path, string contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new AppendAllTextOperation(this, path, contents, encoding).ExecuteAsync(cancellationToken);
        }
#endif

        /// <inheritdoc cref="System.IO.File.AppendText(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public StreamWriter AppendText(string path)
        {
            return new AppendTextOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Copy(string, string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void Copy(string sourceFileName, string destFileName)
        {
            new CopyOperation(this, sourceFileName, destFileName).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Copy(string, string, bool)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            new CopyOperation(this, sourceFileName, destFileName, overwrite).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Create(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Stream Create(string path)
        {
            return new CreateOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Create(string, int)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Stream Create(string path, int bufferSize)
        {
            return new CreateOperation(this, path, bufferSize).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Create(string, int, FileOptions)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Stream Create(string path, int bufferSize, FileOptions options)
        {
            return new CreateOperation(this, path, bufferSize, options).Execute();
        }

        /// <inheritdoc cref="System.IO.File.CreateText(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public StreamWriter CreateText(string path)
        {
            return new CreateTextOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Decrypt(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void Decrypt(string path)
        {
            new DecryptOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Delete(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void Delete(string path)
        {
            new DeleteOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Encrypt(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void Encrypt(string path)
        {
            new EncryptOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Exists(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public bool Exists(string path)
        {
            return new ExistsOperation(this, path).Execute();
        }

#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
#if NET461 || NET5_0
        /// <inheritdoc cref="System.IO.File.GetAccessControl(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
#else
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
#endif
        public FileSecurity GetAccessControl(string path)
        {
            return new GetAccessControlOperation(this, path).Execute();
        }

#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
#if NET461 || NET5_0
        /// <inheritdoc cref="System.IO.File.GetAccessControl(string, AccessControlSections)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
#else
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
#endif
        public FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return new GetAccessControlOperation(this, path, includeSections).Execute();
        }

        /// <inheritdoc cref="System.IO.File.GetAttributes(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public FileAttributes GetAttributes(string path)
        {
            return new GetAttributesOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.GetCreationTime(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public DateTime GetCreationTime(string path)
        {
            return new GetCreationTimeOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.GetCreationTimeUtc(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public DateTime GetCreationTimeUtc(string path)
        {
            return new GetCreationTimeOperation(this, path, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.File.GetLastAccessTime(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public DateTime GetLastAccessTime(string path)
        {
            return new GetLastAccessTimeOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.GetLastAccessTimeUtc(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public DateTime GetLastAccessTimeUtc(string path)
        {
            return new GetLastAccessTimeOperation(this, path, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.File.GetLastWriteTime(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public DateTime GetLastWriteTime(string path)
        {
            return new GetLastWriteTimeOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.GetLastAccessTimeUtc(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public DateTime GetLastWriteTimeUtc(string path)
        {
            return new GetLastWriteTimeOperation(this, path, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Move(string, string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void Move(string sourceFileName, string destFileName)
        {
            new MoveOperation(this, sourceFileName, destFileName).Execute();
        }

#if NET5_0
        /// <inheritdoc cref="System.IO.File.Move(string, string, bool)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void Move(string sourceFileName, string destFileName, bool overwrite)
        {
            new MoveOperation(this, sourceFileName, destFileName, overwrite).Execute();
        }
#endif

        /// <inheritdoc cref="System.IO.File.Open(string, FileMode)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Stream Open(string path, FileMode mode)
        {
            return new OpenOperation(this, path, mode).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Open(string, FileMode, FileAccess)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Stream Open(string path, FileMode mode, FileAccess access)
        {
            return new OpenOperation(this, path, mode, access).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Open(string, FileMode, FileAccess, FileShare)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return new OpenOperation(this, path, mode, access, share).Execute();
        }

        /// <inheritdoc cref="System.IO.File.OpenRead(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public Stream OpenRead(string path)
        {
            return new OpenReadOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.OpenText(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public StreamReader OpenText(string path)
        {
            return new OpenTextOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.OpenWrite(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Stream OpenWrite(string path)
        {
            return new OpenWriteOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.ReadAllBytes(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public byte[] ReadAllBytes(string path)
        {
            return new ReadAllBytesOperation(this, path).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        /// <inheritdoc cref="System.IO.File.ReadAllBytesAsync(string, CancellationToken)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
        {
            return new ReadAllBytesOperation(this, path).ExecuteAsync(cancellationToken);
        }
#endif

        /// <inheritdoc cref="System.IO.File.ReadAllLines(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public string[] ReadAllLines(string path)
        {
            return new ReadAllLinesOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.ReadAllLines(string, Encoding)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public string[] ReadAllLines(string path, Encoding encoding)
        {
            return new ReadAllLinesOperation(this, path, encoding).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        public Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default)
        {
            return new ReadAllLinesOperation(this, path).ExecuteAsync(cancellationToken);
        }

        public Task<string[]> ReadAllLinesAsync(string path, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new ReadAllLinesOperation(this, path, encoding).ExecuteAsync(cancellationToken);
        }
#endif

        /// <inheritdoc cref="System.IO.File.ReadAllText(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public string ReadAllText(string path)
        {
            return new ReadAllTextOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.ReadAllText(string, Encoding)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public string ReadAllText(string path, Encoding encoding)
        {
            return new ReadAllTextOperation(this, path, encoding).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        /// <inheritdoc cref="System.IO.File.ReadAllTextAsync(string, CancellationToken)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
        {
            return new ReadAllTextOperation(this, path).ExecuteAsync(cancellationToken);
        }

        /// <inheritdoc cref="System.IO.File.ReadAllTextAsync(string, Encoding, CancellationToken)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public Task<string> ReadAllTextAsync(string path, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new ReadAllTextOperation(this, path, encoding).ExecuteAsync(cancellationToken);
        }
#endif

        /// <inheritdoc cref="System.IO.File.ReadLines(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public IEnumerable<string> ReadLines(string path)
        {
            return new ReadLinesOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.File.ReadLines(string, Encoding)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="info"]/*' />
        public IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            return new ReadLinesOperation(this, path, encoding).Execute();
        }

        /// <inheritdoc cref="System.IO.File.Replace(string, string, string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            new ReplaceOperation(this, sourceFileName, destinationFileName, destinationBackupFileName)
                .Execute();
        }

        /// <inheritdoc cref="System.IO.File.Replace(string, string, string, bool)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName,
            bool ignoreMetadataErrors)
        {
            new ReplaceOperation(this, sourceFileName, destinationFileName, destinationBackupFileName,
                ignoreMetadataErrors)
                .Execute();
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        /// <inheritdoc cref="System.IO.File.SetAccessControl(string, FileSecurity)" />
#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        public void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            new SetAccessControlOperation(this, path, fileSecurity).Execute();
        }

        /// <inheritdoc cref="System.IO.File.SetAttributes(string, FileAttributes)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void SetAttributes(string path, FileAttributes fileAttributes)
        {
            new SetAttributesOperation(this, path, fileAttributes).Execute();
        }

        /// <inheritdoc cref="System.IO.File.SetCreationTime(string, DateTime)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void SetCreationTime(string path, DateTime creationTime)
        {
            new SetCreationTimeOperation(this, path, creationTime).Execute();
        }

        /// <inheritdoc cref="System.IO.File.SetCreationTimeUtc(string, DateTime)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            new SetCreationTimeOperation(this, path, creationTimeUtc, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.File.SetLastAccessTime(string, DateTime)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            new SetLastAccessTimeOperation(this, path, lastAccessTime).Execute();
        }

        /// <inheritdoc cref="System.IO.File.SetLastAccessTimeUtc(string, DateTime)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            new SetLastAccessTimeOperation(this, path, lastAccessTimeUtc, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.File.SetLastWriteTime(string, DateTime)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            new SetLastWriteTimeOperation(this, path, lastWriteTime).Execute();
        }

        /// <inheritdoc cref="System.IO.File.SetLastWriteTimeUtc(string, DateTime)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            new SetLastWriteTimeOperation(this, path, lastWriteTimeUtc, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.File.WriteAllBytes(string, byte[])" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void WriteAllBytes(string path, byte[] bytes)
        {
            new WriteAllBytesOperation(this, path, bytes).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        /// <inheritdoc cref="System.IO.File.WriteAllBytesAsync(string, byte[], CancellationToken)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
        {
            return new WriteAllBytesOperation(this, path, bytes).ExecuteAsync(cancellationToken);
        }
#endif

        /// <inheritdoc cref="System.IO.File.WriteAllLines(string, IEnumerable{string})" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void WriteAllLines(string path, IEnumerable<string> contents)
        {
            new WriteAllLinesOperation(this, path, contents).Execute();
        }

        /// <inheritdoc cref="System.IO.File.WriteAllLines(string, IEnumerable{string}, Encoding)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            new WriteAllLinesOperation(this, path, contents, encoding).Execute();
        }

        /// <inheritdoc cref="System.IO.File.WriteAllLines(string, string[])" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void WriteAllLines(string path, string[] contents)
        {
            new WriteAllLinesOperation(this, path, contents).Execute();
        }

        /// <inheritdoc cref="System.IO.File.WriteAllLines(string, string[], Encoding)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            new WriteAllLinesOperation(this, path, contents, encoding).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        /// <inheritdoc cref="System.IO.File.WriteAllLinesAsync(string, IEnumerable{string}, CancellationToken)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Task WriteAllLinesAsync(string path, IEnumerable<string> contents,
            CancellationToken cancellationToken = default)
        {
            return new WriteAllLinesOperation(this, path, contents).ExecuteAsync(cancellationToken);
        }

        /// <inheritdoc cref="System.IO.File.WriteAllLinesAsync(string, IEnumerable{string}, Encoding, CancellationToken)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new WriteAllLinesOperation(this, path, contents, encoding).ExecuteAsync(cancellationToken);
        }

        // TODO: check if the below `inheritdoc` results in proper documentation, as no `string[]` variant exists on `System.IO.File`.
        /// <inheritdoc cref="System.IO.File.WriteAllLinesAsync(string, IEnumerable{string}, CancellationToken)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Task WriteAllLinesAsync(string path, string[] contents,
            CancellationToken cancellationToken = default)
        {
            return new WriteAllLinesOperation(this, path, contents).ExecuteAsync(cancellationToken);
        }

        // TODO: check if the below `inheritdoc` results in proper documentation, as no `string[]` variant exists on `System.IO.File`.
        /// <inheritdoc cref="System.IO.File.WriteAllLinesAsync(string, IEnumerable{string}, Encoding, CancellationToken)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Task WriteAllLinesAsync(string path, string[] contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new WriteAllLinesOperation(this, path, contents, encoding).ExecuteAsync(cancellationToken);
        }
#endif

        /// <inheritdoc cref="System.IO.File.WriteAllText(string, string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void WriteAllText(string path, string contents)
        {
            new WriteAllTextOperation(this, path, contents).Execute();
        }

        /// <inheritdoc cref="System.IO.File.WriteAllText(string, string, Encoding)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public void WriteAllText(string path, string contents, Encoding encoding)
        {
            new WriteAllTextOperation(this, path, contents, encoding).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        /// <inheritdoc cref="System.IO.File.WriteAllTextAsync(string, string, CancellationToken)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default)
        {
            return new WriteAllTextOperation(this, path, contents).ExecuteAsync(cancellationToken);
        }

        /// <inheritdoc cref="System.IO.File.WriteAllTextAsync(string, string, Encoding, CancellationToken)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileOperation" and @type="modify"]/*' />
        public Task WriteAllTextAsync(string path, string contents, Encoding encoding,
            CancellationToken cancellationToken = default)
        {
            return new WriteAllTextOperation(this, path, contents, encoding).ExecuteAsync(cancellationToken);
        }
#endif
    }
}