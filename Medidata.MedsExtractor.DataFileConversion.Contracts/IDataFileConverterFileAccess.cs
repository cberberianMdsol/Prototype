using System.Text;

namespace Medidata.MedsExtractor.DataFileConversion.Contracts
{
    public interface IDataFileConverterFileAccess
    {
        string GetFile(string requestUri);
        void PutFile(string fileData, string requestResponseUri);
    }
}