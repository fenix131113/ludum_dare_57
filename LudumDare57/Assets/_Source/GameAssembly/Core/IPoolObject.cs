namespace Core
{
    public interface IPoolObject
    {
        public void InitPool(DictionaryObjectPool pool);
        public void ReturnToPool();
    }
}