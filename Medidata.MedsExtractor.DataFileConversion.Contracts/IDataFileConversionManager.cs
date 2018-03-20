using System;

namespace Medidata.MedsExtractor.DataFileConversion.Contracts
{
    public interface IDataFileConversionManager
    {
        DataToFileConversionResponse ConvertToCsv(DataToFileConversionRequest request);
    }
}
