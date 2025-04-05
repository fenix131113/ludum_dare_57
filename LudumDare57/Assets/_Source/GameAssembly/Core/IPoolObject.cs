namespace Core
{
    public interface IPoolObject
    {
        public void PoolInit(DictionaryObjectPool pool);
        public void ReturnToPool();
    }
}