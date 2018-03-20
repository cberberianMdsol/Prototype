namespace Medidata.MedsExtractor.DataFileConversion.Contracts
{
    public class DataToFileConversionRequest
    {
        public string InputFileUri { get; set; }
        public string SessionId { get; set; }
        public string OutputFileUri { get; set; }
        public string Method { get; set; }
    }
}