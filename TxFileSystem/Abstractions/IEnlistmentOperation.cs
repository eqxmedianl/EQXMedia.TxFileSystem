namespace EQXMedia.TxFileSystem.Abstractions
{
    public interface IEnlistmentOperation
    {
        void Commit();

        void Rollback();
    }
}