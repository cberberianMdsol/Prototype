namespace Medidata.MedsExtractor.DataFileConversion.Contracts
{
    public interface IDataFileTranformationEngine
    {
        string TranformJsonToCsv(string medsData);
    }
}