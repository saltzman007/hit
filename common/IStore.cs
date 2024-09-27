namespace common
{
    public interface IStore
    {
        void Init();
        string WriteStoreFile(string filename);
        byte[] ReadStoreFile(string sha1);
    }
}

