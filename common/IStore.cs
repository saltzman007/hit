namespace common
{
    public interface IStore
    {
        void Store(object o);
        void StoreFile(string filename);
       // object Load(string filename);
    }
}

