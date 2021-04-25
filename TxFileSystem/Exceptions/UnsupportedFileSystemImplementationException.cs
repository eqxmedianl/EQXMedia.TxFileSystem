namespace EQXMedia.TxFileSystem.Exceptions
{
    using System;
    using System.IO.Abstractions;
    using System.Runtime.Serialization;

    /// <summary>
    ///   The <see cref="UnsupportedFileSystemImplementationException" /> is thrown whenever an unsupported 
    ///   implementation of 
    ///   <see href="https://github.com/System-IO-Abstractions/System.IO.Abstractions/blob/main/src/System.IO.Abstractions/IFileSystem.cs"
    ///     alt="System.IO.Abstractions/IFileSystem.cs at main - System-IO-Abstractions/System.IO.Abstractions" target="_blank">
    ///     System.IO.Abstractions.IFileSystem</see> is trying to be wrapped inside the <see cref="TxFileSystem" />.
    /// </summary>
    /// <remarks>Currently this exception is only thrown when one <see cref="TxFileSystem" /> instance is tried to be wrapped inside another.</remarks>
    [Serializable]
    public class UnsupportedFileSystemImplementationException : Exception
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="UnsupportedFileSystemImplementationException" /> class.
        /// </summary>
        /// <param name="fileSystem">The implementation of <see href="https://github.com/System-IO-Abstractions/System.IO.Abstractions/blob/main/src/System.IO.Abstractions/IFileSystem.cs">System.IO.Abstractions.IFileSystem</see> that was not supposed to be wrapped.</param>
        public UnsupportedFileSystemImplementationException(IFileSystem fileSystem)
            : base(CreateErrorMessage(fileSystem))
        {
        }

        private static string CreateErrorMessage(IFileSystem fileSystem)
        {
            return string.Format("The IFileSystem implementation is a {0} which is not supposed to be wrapped by TxFileSystem.",
                fileSystem.GetType().Name);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="UnsupportedFileSystemImplementationException" /> class from serialized data.
        /// </summary>
        /// <param name="info">The serialization information object holding the serialized object data in the name-value form.</param>
        /// <param name="context">The contextual information about the source or destination of the exception.</param>
        /// <exception cref="ArgumentNullException">The <c>info</c> parameter is <c>null</c>.</exception>
        protected UnsupportedFileSystemImplementationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
