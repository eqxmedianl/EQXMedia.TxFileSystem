namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Operations.FileStreams;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.IO;
    using System.IO.Abstractions;

#if SUPPORTED_OS_PLATFORM
    using System.Runtime.Versioning;
#endif

    /// <summary>
    ///   Transactional file stream exposes a factory to create a <see cref="System.IO.Stream" /> for a file, 
    ///   supporting both synchronous and asynchronous read and write operations.
    /// </summary>
    /// <remarks>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="true"]/remarks/*' />
    ///   <note type="note">
    ///     <c>TxFileStream</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileStream" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </note>
    /// </remarks>
    [Serializable]
    public class TxFileStream : IFileStreamFactory
    {
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@context="constructor" and @param="TxFileSystem"]/*' />
        internal TxFileStream(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="FileSystem"]/*' />
        public IFileSystem FileSystem => this.TxFileSystem;

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="TxFileSystem"]/*' />
        internal TxFileSystem TxFileSystem { get; set; }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="path"]/*' />
        /// <inheritdoc cref="System.IO.FileStream(string, FileMode)"/>
        public Stream Create(string path, FileMode mode)
        {
            return new CreateFromPathOperation(this, path, mode).Execute();
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="path"]/*' />
        /// <inheritdoc cref="System.IO.FileStream(string, FileMode, FileAccess)"/>
        public Stream Create(string path, FileMode mode, FileAccess access)
        {
            return new CreateFromPathOperation(this, path, mode, access).Execute();
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="path"]/*' />
        /// <inheritdoc cref="System.IO.FileStream(string, FileMode, FileAccess, FileShare)"/>
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return new CreateFromPathOperation(this, path, mode, access, share).Execute();
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="path"]/*' />
        /// <inheritdoc cref="System.IO.FileStream(string, FileMode, FileAccess, FileShare, int)"/>
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
        {
            return new CreateFromPathOperation(this, path, mode, access, share, bufferSize).Execute();
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="path"]/*' />
        /// <inheritdoc cref="System.IO.FileStream(string, FileMode, FileAccess, FileShare, int, FileOptions)"/>
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize,
            FileOptions options)
        {
            return new CreateFromPathOperation(this, path, mode, access, share, bufferSize, options).Execute();
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="path"]/*' />
        /// <inheritdoc cref="System.IO.FileStream(string, FileMode, FileAccess, FileShare, int)"/>
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize,
            bool useAsync)
        {
            return new CreateFromPathOperation(this, path, mode, access, share, bufferSize, useAsync: useAsync)
                .Execute();
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="handle"]/*' />
        /// <inheritdoc cref="System.IO.FileStream(SafeFileHandle, FileAccess)"/>
        public Stream Create(SafeFileHandle handle, FileAccess access)
        {
            return new CreateFromSafeFileHandleOperation(this, handle, access).Execute();
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="handle"]/*' />
        /// <inheritdoc cref="System.IO.FileStream(SafeFileHandle, FileAccess, int)"/>
        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize)
        {
            return new CreateFromSafeFileHandleOperation(this, handle, access, bufferSize).Execute();
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="handle"]/*' />
        /// <inheritdoc cref="System.IO.FileStream(SafeFileHandle, FileAccess, int, bool)"/>
        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
        {
            return new CreateFromSafeFileHandleOperation(this, handle, access, bufferSize, isAsync: isAsync).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(IntPtr, FileAccess)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="handle"]/*' />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @only_obsolete="true"]/*' />
#if SUPPORTED_OS_PLATFORM
        [SupportedOSPlatform("windows")]
#endif
        [Obsolete("Even though FileStream.Create(IntPtr handle, FileAccess access) is deprecated it is still part of the interface")]
        public Stream Create(IntPtr handle, FileAccess access)
        {
            return new CreateFromFileHandleOperation(this, handle, access).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(IntPtr, FileAccess, bool)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="handle"]/*' />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @only_obsolete="true"]/*' />
#if SUPPORTED_OS_PLATFORM
        [SupportedOSPlatform("windows")]
#endif
        [Obsolete("Even though FileStream.Create(IntPtr handle, FileAccess access, bool ownsHandle) is deprecated it is still part of the interface")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle)
        {
            return new CreateFromFileHandleOperation(this, handle, access, ownsHandle).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(IntPtr, FileAccess, bool, int)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="handle"]/*' />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @only_obsolete="true"]/*' />
#if SUPPORTED_OS_PLATFORM
        [SupportedOSPlatform("windows")]
#endif
        [Obsolete("Even though FileStream.Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize) is deprecated it is still part of the interface")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
        {
            return new CreateFromFileHandleOperation(this, handle, access, ownsHandle, bufferSize).Execute();
        }

        /// <returns>The newly created instance of the <see cref="FileStream" /> class for the specified file <c>handle</c>.</returns>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileStreamOperation" and @type="create" and @from="handle"]/*' />
        /// <inheritdoc cref="System.IO.FileStream(IntPtr, FileAccess, bool, int, bool)"/>
#if SUPPORTED_OS_PLATFORM
        [SupportedOSPlatform("windows")]
#endif
        [Obsolete("Even though FileStream.Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync) is deprecated it is still part of the interface")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
        {
            return new CreateFromFileHandleOperation(this, handle, access, ownsHandle, bufferSize, isAsync: isAsync)
                .Execute();
        }
    }
}