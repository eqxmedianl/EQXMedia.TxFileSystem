namespace EQXMedia.TxFileSystem.Operations
{
    /// <summary>
    ///   <para>
    ///      The <c>OperationType</c> is used to specify the type of operation.
    ///   </para>
    ///   <para>
    ///     Backups and journalizing certain types of operations is guarded against by 
    ///     <see cref="EQXMedia.TxFileSystem.Operations.OperationBackupGuard" /> and
    ///     <see cref="EQXMedia.TxFileSystem.Operations.OperationRollbackGuard" />.
    ///   </para>
    /// </summary>
    /// <remarks>
    ///   The type of an operation determines whether or not the operation will be 
    ///   journalized, and needs to be backed up and restored.
    /// </remarks>
    public enum OperationType
    {
        /// <summary>
        ///   Used to specify that an operation creates a file system resource.
        /// </summary>
        Create,

        /// <summary>
        ///   Used to specify that an operation creates a symbolic link.
        /// </summary>
        CreateSymlink,

        /// <summary>
        ///   Used to specify that an operation creates a <see cref="System.IO.FileStream" />.
        /// </summary>
        /// <remarks>
        ///   The <see cref="System.IO.FileStream" /> can be used to modify files.
        /// </remarks>
        CreateFileStream,

        /// <summary>
        ///   Used to specify that an operation deletes a file system resource.
        /// </summary>
        Delete,

        /// <summary>
        ///   Used to specify that an operation duplicates a file system resource.
        /// </summary>
        Duplicate,

        /// <summary>
        ///   Used to specify that an operation modifies a file system resource.
        /// </summary>
        Modify,

        /// <summary>
        ///   Used to specify that an operation modifies information about a file system resource.
        /// </summary>
        ModifyInfo,

        /// <summary>
        ///   Used to specify that an operation moves a file system resource.
        /// </summary>
        Move,

        /// <summary>
        ///   Used to specify that an operation returns information about a file system resource.
        /// </summary>
        /// <remarks>
        ///   In this case no modifications are made to file system resources. Hence these kind
        ///   of operations are not journalized.
        /// </remarks>
        Info,

        /// <summary>
        ///   Used to specify that an operation navigates the <c>System.IO.Abstractions.IFileSystem</c>.
        /// </summary>
        /// <remarks>
        ///   This is a journalizing operation, even though no modifications are made to file system
        ///   resources. When a rollback occurs the navigation is reversed.
        /// </remarks>
        Navigate,

        /// <summary>
        ///   Used to specify that an operation opens a file.
        /// </summary>
        /// <remarks>
        ///   In this case modifications can be made to file system resources. Hence these kind of operations
        ///   are not journalized.
        /// </remarks>
        Open,

        /// <summary>
        ///   Used to specify that an operation reads from a file.
        /// </summary>
        /// <remarks>
        ///   In this case no modifications are made to file system resources. Hence these kind of operations 
        ///   are not journalized.
        /// </remarks>
        Read
    }
}
