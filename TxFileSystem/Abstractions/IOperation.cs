namespace EQXMedia.TxFileSystem.Abstractions
{
    using global::EQXMedia.TxFileSystem.Operations;

    public interface IOperation
    {
        OperationType OperationType { get; }

        void Journalize(IOperation operation);
    }
}