namespace EQXMedia.TxFileSystem.Abstractions
{
    internal interface IReturningOperation<out TReturn> : IOperation
    {
        public abstract TReturn Execute();
    }
}
