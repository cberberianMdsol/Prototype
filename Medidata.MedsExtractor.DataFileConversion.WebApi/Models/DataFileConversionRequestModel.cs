using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medidata.MedsExtractor.DataFileConversion.WebApi.Models
{
    public class DataFileConversionRequestModel
    {
        public string ResponseUri { get; set; }
        public string Uri { get; set; }
        public string SessionId { get; set; }
    }
}
