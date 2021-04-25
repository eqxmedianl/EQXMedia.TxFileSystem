namespace EQXMedia.TxFileSystem.Exceptions
{
    using System;
    using System.IO.Abstractions;
    using System.Runtime.Serialization;

    [Serializable]
    public class UnsupportedFileSystemImplementationException : Exception
    {
        public UnsupportedFileSystemImplementationException(IFileSystem fileSystem)
            : base(CreateErrorMessage(fileSystem))
        {
        }

        private static string CreateErrorMessage(IFileSystem fileSystem)
        {
            return string.Format("The IFileSystem implementation is {0} which is not supported to be wrapped by TxFileSystem.",
                fileSystem.GetType().Name);
        }

        protected UnsupportedFileSystemImplementationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
