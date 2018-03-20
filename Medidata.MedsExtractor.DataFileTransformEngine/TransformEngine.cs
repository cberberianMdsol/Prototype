using System;
using Medidata.MedsExtractor.DataFileConversion.Contracts;

namespace Medidata.MedsExtractor.DataFileTransformEngine
{
    public class TransformEngine : IDataFileTranformationEngine
    {
        public string TranformJsonToCsv(string medsData)
        {
            return "data1,data2,data3";
        }
    }
}
