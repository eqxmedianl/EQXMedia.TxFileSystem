namespace EQXMedia.TxFileSystem.Abstractions
{
    internal interface IReturningOperation<out TReturn> : IOperation
    {
        TReturn Execute();
    }
}
