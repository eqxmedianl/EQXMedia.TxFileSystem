namespace EQXMedia.TxFileSystem.Abstractions
{
    public interface IEnlistmentOperation
    {
        public abstract void Commit();

        public abstract void Rollback();
    }
}