using Medidata.MedsExtractor.DataFileConversion.Contracts;
using Moq;
using NUnit.Framework;

namespace Medidata.MedsExtractor.DataFileConversionManager.UniTests
{
    public class DataFileConversionManagerFixture
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var dataFileConverterCacheAccess = new Mock<IDataFileConverterCacheAccess>();
            var dataFileConverterFileAccess = new Mock<IDataFileConverterFileAccess>();
            var dataFileTranformationEngine = new Mock<IDataFileTranformationEngine>();
            var mgr = new DataFileConversionManager(dataFileConverterCacheAccess.Object, dataFileConverterFileAccess.Object, dataFileTranformationEngine.Object);
            var request = new DataToFileConversionRequest();
            mgr.ConvertToCsv(request);
        }
    }
}