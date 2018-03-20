using System;
using System.Text;
using Medidata.MedsExtractor.DataFileConversion.Contracts;
using Medidata.MedsExtractor.S3Utility;

namespace Medidata.MedsExtractor.FileAccess
{
    public class DataFileConverterFileAccess : IDataFileConverterFileAccess
    {
        private readonly IS3Client _s3Client;

        public DataFileConverterFileAccess(IS3Client s3Client)
        {
            _s3Client = s3Client;
        }

        public string GetFile(string requestUri)
        {
            return "head1,head2\r\nval1,val1";
        }

        public void PutFile(string fileData, string requestResponseUri)
        {
            var s3ClientRequest = new S3ClientRequest {Uri = requestResponseUri};
            _s3Client.Put(s3ClientRequest);
        }
    }
}
