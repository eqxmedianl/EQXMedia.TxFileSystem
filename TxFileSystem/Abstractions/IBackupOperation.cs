namespace EQXMedia.TxFileSystem.Abstractions
{
    using global::EQXMedia.TxFileSystem.Operations;

    /// <summary>
    ///   Operations that should be journalized, and support backup and restore, should implement
    ///   this interface.
    /// </summary>
    /// <see cref="EQXMedia.TxFileSystem.Operations.Files.FileBackupOperation" />
    /// <see cref="EQXMedia.TxFileSystem.Operations.Directories.DirectoryBackupOperation" />
    /// <see cref="EQXMedia.TxFileSystem.Operations.FileStreams.FileStreamOperation" />
    public interface IBackupOperation
    {
        /// <inheritdoc cref="EQXMedia.TxFileSystem.Operations.OperationType" />
        OperationType OperationType { get; }

        /// <summary>
        ///   The path at which the backup is created by the operation that implements 
        ///   <see cref="IBackupOperation" />.
        /// </summary>
        string BackupPath { get; }

        /// <summary>
        ///   Performs the backup that has to be created by the operation that implements 
        ///   <see cref="IBackupOperation" />.
        /// </summary>
        void Backup();

        /// <summary>
        ///   Deletes the backup that has been created by the operation that implements 
        ///   <see cref="IBackupOperation" />.
        /// </summary>
        void Delete();

        /// <summary>
        ///   Restores the backup that has been created by the operation that implements 
        ///   <see cref="IBackupOperation" />.
        /// </summary>
        void Restore();
    }
}