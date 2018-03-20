namespace Medidata.MedsExtractor.RedisUtility
{
    public interface IRedisCache
    {
        string Get(string requestSessionId);
        string Save(string key, string data);
    }
}