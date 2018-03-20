using Medidata.MedsExtractor.DataFileConversion.Contracts;
using Medidata.MedsExtractor.DataFileTransformEngine;
using Medidata.MedsExtractor.FileAccess;
using Medidata.MedsExtractor.RedisUtility;
using Medidata.MedsExtractor.S3Utility;
using Moq;
using NUnit.Framework;

namespace Medidata.MedsExtractor.DataFileConversion.CollaborationTests
{
    public class CollaborationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var redisCache = new Mock<IRedisCache>();
            var cacheAccess = new CacheAccess.CacheAccess(redisCache.Object);
            var s3Client = new Mock<IS3Client>();
            var fileAccess = new DataFileConverterFileAccess(s3Client.Object);
            var dataFileTranform = new TransformEngine();
            var dfcm = new DataFileConversionManager.DataFileConversionManager(cacheAccess, fileAccess, dataFileTranform);
            var request = new DataToFileConversionRequest();

            dfcm.ConvertToCsv(request);

        }
    }
}