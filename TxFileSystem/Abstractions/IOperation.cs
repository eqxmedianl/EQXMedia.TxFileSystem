namespace EQXMedia.TxFileSystem.Abstractions
{
    using global::EQXMedia.TxFileSystem.Operations;

    public interface IOperation
    {
        public OperationType OperationType { get; }

        public abstract void Journalize(IOperation operation);
    }
}