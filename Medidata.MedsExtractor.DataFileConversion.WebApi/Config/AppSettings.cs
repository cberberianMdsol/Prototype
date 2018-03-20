using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.MAuth.AspNetCore;

namespace Medidata.MedsExtractor.DataFileConversion.WebApi.Config
{
    public class AppConfiguration
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        public AppSettings AppSettings { get; set; }

    }

    public class ConnectionStrings
    {
        public string MEDbContext { get; set; }
    }

    public class AppSettings
    {
        public string ApplicationName { get; set; }

        public MAuthSettings MAuth { get; set; }

        public ZipkinSettings Zipkin { get; set; }
    }

    public class MAuthSettings
    {
        public Guid Uuid { get; set; }

        public string PrivateKey { get; set; }

        public string ServiceUrl { get; set; }

        public bool? HideExceptionsAndReturnForbidden { get; set; }

        public IEnumerable<string> EnabledPaths { get; set; }
    }

    public class ZipkinSettings
    {
        public float SamplingRate { get; set; }

        public Uri BaseUri { get; set; }

        public IEnumerable<string> EnabledPaths { get; set; }
    }
}