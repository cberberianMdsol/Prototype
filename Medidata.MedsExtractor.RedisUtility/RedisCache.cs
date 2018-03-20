using System;

namespace Medidata.MedsExtractor.RedisUtility
{
    public class RedisCache : IRedisCache
    {
        public string Get(string requestSessionId)
        {
            throw new NotImplementedException();
        }

        public string Save(string key, string data)
        {
            return string.Empty;
        }
    }
}
