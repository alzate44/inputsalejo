using inputsalejo.Common.Models;
using inputsalejo.Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace inputsalejo.Tests.Helpers
{
    public class TestFactory
    {
        public static InputEntity GetInputEntity()
        {
            return new InputEntity
            {
                ETag = "*",
                PartitionKey = "INPUT",
                RowKey = Guid.NewGuid().ToString(),
                InputDate = DateTime.UtcNow,
                Consolidated = false,
                EmployeeId = 8,
                Type = 1
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid inputId, Input inputRequest) //Update
        {
            string request = JsonConvert.SerializeObject(inputRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{inputId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid inputId) //Delete or GetById
        {  
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{inputId}",
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Input inputRequest) //Create
        {
            string request = JsonConvert.SerializeObject(inputRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
            };
        }

        public static DefaultHttpRequest CreateHttpRequest() //Get all
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }

        
        public static Input GetInputRequest()
        {
            return new Input
            {
                
                InputDate = DateTime.UtcNow,
                Consolidated = false,
                EmployeeId = 8,
                Type = 0
                
                
            };
        }


        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;
            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}
