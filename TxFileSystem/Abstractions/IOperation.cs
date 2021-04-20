namespace EQXMedia.TxFileSystem.Abstractions
{
    using global::EQXMedia.TxFileSystem.Operations;

    internal interface IOperation
    {
        OperationType OperationType { get; }

        void Journalize(IOperation operation);
    }
}