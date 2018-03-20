using Medidata.MedsExtractor.DataFileConversion.Contracts;

namespace Medidata.MedsExtractor.DataFileConversionManager
{
    public class DataFileConversionManager : IDataFileConversionManager
    {
        private readonly IDataFileConverterCacheAccess _cacheAccess;
        private readonly IDataFileConverterFileAccess _fileAccess;
        private readonly IDataFileTranformationEngine _dataFileTranformationEngine;

        public DataFileConversionManager(IDataFileConverterCacheAccess cacheAccess, IDataFileConverterFileAccess fileAccess, IDataFileTranformationEngine dataFileTranformationEngine)
        {
            _cacheAccess = cacheAccess;
            _fileAccess = fileAccess;
            _dataFileTranformationEngine = dataFileTranformationEngine;
        }

        public DataToFileConversionResponse ConvertToCsv(DataToFileConversionRequest request)
        {
            var cacheInput = _cacheAccess.GetMedsDataCache($"{request.SessionId}") ?? new CacheResult();
            
            string medsData;

            if (cacheInput.Exists)
            {
                medsData = cacheInput.Value;
            }
            else
            {
                medsData = _fileAccess.GetFile(request.InputFileUri);
                _cacheAccess.SaveMedsData(request.SessionId, medsData);
            }

            //No meds data associated with the session id and request input file 
            if (medsData == null)
            {
                return new DataToFileConversionResponse
                {
                    Status = DataToFileConversionResponseStatus.MedsDataNotFound
                };
            }

            _fileAccess.PutFile(_dataFileTranformationEngine.TranformJsonToCsv(medsData), request.OutputFileUri);

            return new DataToFileConversionResponse
            {
                Status = DataToFileConversionResponseStatus.Complete
            };

        }
    }
}
