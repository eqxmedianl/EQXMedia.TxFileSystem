namespace EQXMedia.TxFileSystem.Abstractions
{
    internal interface IEnlistmentOperation
    {
        void Commit();

        void Rollback();
    }
}