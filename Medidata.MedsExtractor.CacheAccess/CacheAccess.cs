using System;
using Medidata.MedsExtractor.DataFileConversion.Contracts;
using Medidata.MedsExtractor.RedisUtility;

namespace Medidata.MedsExtractor.CacheAccess
{
    public class CacheAccess : IDataFileConverterCacheAccess
    {
        private readonly IRedisCache _redisCache;

        public CacheAccess(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }

        public CacheResult GetFileData(string cacheId)
        {
            return new CacheResult
            {
                Value = _redisCache.Get(cacheId)
            };
        }

        public string SaveMedsData(string key, string data)
        {
            return _redisCache.Save($"meds{key}", data);
        }

        public CacheResult GetMedsDataCache(string s)
        {
            return new CacheResult {Exists = false, Value = null};
        }
    }
}
