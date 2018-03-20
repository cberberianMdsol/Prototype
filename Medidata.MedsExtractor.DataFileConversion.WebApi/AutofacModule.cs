using Autofac;
using Medidata.MDLogging;
using Medidata.MedsExtractor.DataFileConversion.Contracts;
using Medidata.MedsExtractor.DataFileTransformEngine;
using Medidata.MedsExtractor.FileAccess;
using Medidata.MedsExtractor.RedisUtility;
using Medidata.MedsExtractor.S3Utility;

namespace Medidata.MedsExtractor.DataFileConversion.WebApi
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            
            builder.Register(x => new RedisCache()).As<IRedisCache>();
            builder.Register(x => new CacheAccess.CacheAccess(x.Resolve<IRedisCache>())).As<IDataFileConverterCacheAccess>();
            
            builder.Register(x => new S3Client()).As<IS3Client>();
            builder.Register(x => new DataFileConverterFileAccess(
                    x.Resolve<IS3Client>()))
                .As<IDataFileConverterFileAccess>();
            builder.Register(x => new TransformEngine()).As<IDataFileTranformationEngine>();
            builder.Register(x => new DataFileConversionManager.DataFileConversionManager(
                    x.Resolve<IDataFileConverterCacheAccess>(), 
                    x.Resolve<IDataFileConverterFileAccess>(),
                    x.Resolve<IDataFileTranformationEngine>()))
                .As<IDataFileConversionManager>();
        }
    }
}