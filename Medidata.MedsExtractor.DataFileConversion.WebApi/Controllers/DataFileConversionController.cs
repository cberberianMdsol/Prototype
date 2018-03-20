using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.MDLogging;
using Medidata.MDLogging.NetCore;
using Medidata.MedsExtractor.DataFileConversion.Contracts;
using Medidata.MedsExtractor.DataFileConversion.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Medidata.MedsExtractor.DataFileConversion.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/DataFileConversion")]
    public class DataFileConversionController : Controller
    {
        private readonly IDataFileConversionManager _dataFileConversionManager;
        private readonly IMDLogger _mdLogger;

        public DataFileConversionController(IDataFileConversionManager dataFileConversionManager, IMDLogger<DataFileConversionController> mdLogger)
        {
            _dataFileConversionManager = dataFileConversionManager;
            _mdLogger = mdLogger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] DataFileConversionRequestModel value)
        {
            _mdLogger.Debug("Entered DataFileConversion Post");
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiBadRequestResponse(ModelState));
                }

                var ret = _dataFileConversionManager.ConvertToCsv(new DataToFileConversionRequest
                {
                    Method = "POST",
                    OutputFileUri = value.ResponseUri,
                    SessionId = value.SessionId,
                    InputFileUri = value.Uri
                });

                return Ok(ret);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            
        }

        [HttpGet]
        public DataToFileConversionResponse Get(string conversionId)
        {
            return _dataFileConversionManager.ConvertToCsv(new DataToFileConversionRequest
            {
                Method = "GET",
                SessionId = conversionId,
            });
        }
    }

    public class ApiBadRequestResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; }

        public ApiBadRequestResponse(ModelStateDictionary modelState)
            : base(400)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();
        }
    }

    public class ApiResponse
    {
        public int StatusCode { get; }

        public string Message { get; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {

                case 404:
                    return "Resource not found";
                case 500:
                    return "An unhandled error occurred";
                default:
                    return null;
            }
        }
    }
}