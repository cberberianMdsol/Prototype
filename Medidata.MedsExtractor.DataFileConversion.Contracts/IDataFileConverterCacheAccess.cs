namespace Medidata.MedsExtractor.DataFileConversion.Contracts
{
    public interface IDataFileConverterCacheAccess
    {
        CacheResult GetFileData(string cacheId);
        string SaveMedsData(string key, string data);
        CacheResult GetMedsDataCache(string s);
    }
}