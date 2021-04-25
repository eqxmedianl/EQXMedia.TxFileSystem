﻿namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Operations.FileStreams;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.IO;
    using System.IO.Abstractions;

#if NET5_0
    using System.Runtime.Versioning;
#endif

    /// <summary>
    ///   Transactional file stream exposes a factory to create a <see cref="System.IO.Stream" /> for a file, 
    ///   supporting both synchronous and asynchronous read and write operations.
    ///   
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="true"]/summary/*' />
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     <c>TxFileStream</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileStream" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </para>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="true"]/remarks/*' />
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

        /// <inheritdoc cref="System.IO.FileStream(string, FileMode)"/>
        public Stream Create(string path, FileMode mode)
        {
            return new CreateFromPathOperation(this, path, mode).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(string, FileMode, FileAccess)"/>
        public Stream Create(string path, FileMode mode, FileAccess access)
        {
            return new CreateFromPathOperation(this, path, mode, access).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(string, FileMode, FileAccess, FileShare)"/>
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return new CreateFromPathOperation(this, path, mode, access, share).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(string, FileMode, FileAccess, FileShare, int)"/>
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
        {
            return new CreateFromPathOperation(this, path, mode, access, share, bufferSize).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(string, FileMode, FileAccess, FileShare, int, FileOptions)"/>
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize,
            FileOptions options)
        {
            return new CreateFromPathOperation(this, path, mode, access, share, bufferSize, options).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(string, FileMode, FileAccess, FileShare, int)"/>
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize,
            bool useAsync)
        {
            return new CreateFromPathOperation(this, path, mode, access, share, bufferSize, useAsync: useAsync)
                .Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(SafeFileHandle, FileAccess)"/>
        public Stream Create(SafeFileHandle handle, FileAccess access)
        {
            return new CreateFromSafeFileHandleOperation(this, handle, access).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(SafeFileHandle, FileAccess, int)"/>
        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize)
        {
            return new CreateFromSafeFileHandleOperation(this, handle, access, bufferSize).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(SafeFileHandle, FileAccess, int, bool)"/>
        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
        {
            return new CreateFromSafeFileHandleOperation(this, handle, access, bufferSize, isAsync: isAsync).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(IntPtr, FileAccess)"/>
#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        [Obsolete("Even though FileStream.Create(IntPtr handle, FileAccess access) is deprecated it is still part of the interface")]
        public Stream Create(IntPtr handle, FileAccess access)
        {
            return new CreateFromFileHandleOperation(this, handle, access).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(IntPtr, FileAccess, bool)"/>
#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        [Obsolete("Even though FileStream.Create(IntPtr handle, FileAccess access, bool ownsHandle) is deprecated it is still part of the interface")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle)
        {
            return new CreateFromFileHandleOperation(this, handle, access, ownsHandle).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(IntPtr, FileAccess, bool, int)"/>
#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        [Obsolete("Even though FileStream.Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize) is deprecated it is still part of the interface")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
        {
            return new CreateFromFileHandleOperation(this, handle, access, ownsHandle, bufferSize).Execute();
        }

        /// <inheritdoc cref="System.IO.FileStream(IntPtr, FileAccess, bool, int, bool)"/>
#if NET5_0
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