namespace Medidata.MedsExtractor.S3Utility
{
    public interface IS3Client
    {
        void Put(S3ClientRequest s3ClientRequest);
    }
}