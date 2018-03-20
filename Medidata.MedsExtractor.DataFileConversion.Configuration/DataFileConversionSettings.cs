using System;
using System.Collections.Generic;

namespace Medidata.MedsExtractor.DataFileConversion.Configuration
{
    public class DataFileConversionSettings
    {
        public Guid MAuthApplicationUuid { get; set; }
        public string MAuthPrivateKey { get; set; }
        public int MAuthAuthenticateRequestTimeoutSeconds { get; set; }
        public IList<string> MAuthBypassPaths { get; set; }
        public bool MAuthHideExceptionsAndReturnUnauthorized { get; set; }
        public string MAuthServiceUrl { get; set; }
    }
}
