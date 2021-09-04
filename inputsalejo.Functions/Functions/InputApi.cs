using inputsalejo.Common.Models;
using inputsalejo.Common.Responses;
using inputsalejo.Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace inputsalejo.Functions.Functions
{
    public static class InputApi
    {
        [FunctionName(nameof(CreateInput))]
        public static async Task<IActionResult> CreateInput(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "input")] HttpRequest req,
            [Table("input", Connection = "AzureWebJobsStorage")] CloudTable inputTable,
            ILogger log)
        {
            log.LogInformation("Recieved a new input.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Input input = JsonConvert.DeserializeObject<Input>(requestBody);

            if (string.IsNullOrEmpty(input?.EmployeeId.ToString()))
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "The field EmployeeId cannot be empty."
                });

            }


            InputEntity inputEntity = new InputEntity
            {
                EmployeeId = input.EmployeeId,
                InputDate = DateTime.UtcNow,
                ETag = "*",
                Type = input.Type,
                Consolidated = false,
                PartitionKey = "INPUT",
                RowKey = Guid.NewGuid().ToString(),


            };

            TableOperation addOperation = TableOperation.Insert(inputEntity);
            await inputTable.ExecuteAsync(addOperation);

            string message = "New input stored in table.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = inputEntity
            });
        }


        [FunctionName(nameof(UpdateInput))]
        public static async Task<IActionResult> UpdateInput(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "input/{id}")] HttpRequest req,
            [Table("input", Connection = "AzureWebJobsStorage")] CloudTable inputTable,
            string id,
            ILogger log)
        {
            log.LogInformation($"Update for input: {id}, received.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Input input = JsonConvert.DeserializeObject<Input>(requestBody);

            //Validate input id
            TableOperation findOperation = TableOperation.Retrieve<InputEntity>("INPUT", id);
            TableResult findResult = await inputTable.ExecuteAsync(findOperation);
            if (findResult.Result == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Input not found."
                });
            }

            //Update Input, update date

            InputEntity inputEntity = (InputEntity)findResult.Result;
            inputEntity.Consolidated = input.Consolidated;
            if (!string.IsNullOrEmpty(input.InputDate.ToString()))
            {
                inputEntity.InputDate = input.InputDate;
            }

            TableOperation addOperation = TableOperation.Replace(inputEntity);
            await inputTable.ExecuteAsync(addOperation);

            string message = $"Input: {id}, updated in table.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = inputEntity
            });
        }

        [FunctionName(nameof(GetAllInputs))]
        public static async Task<IActionResult> GetAllInputs(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "input")] HttpRequest req,
          [Table("input", Connection = "AzureWebJobsStorage")] CloudTable inputTable,
          ILogger log)
        {
            log.LogInformation("Get all inputs received.");

            TableQuery<InputEntity> query = new TableQuery<InputEntity>();
            TableQuerySegment<InputEntity> inputs = await inputTable.ExecuteQuerySegmentedAsync(query, null);


            string message = "Retrieved all inputs.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = inputs
            });
        }

        [FunctionName(nameof(GetInputById))]
        public static IActionResult GetInputById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "input/{id}")] HttpRequest req,
            [Table("input", "INPUT", "{id}", Connection = "AzureWebJobsStorage")] InputEntity inputEntity,
            string id,
            ILogger log)
        {
            log.LogInformation($"Get input by id: {id}, received.");

            if (inputEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Input not found."
                });
            }

            string message = $"Input: {inputEntity.RowKey}, retrieved.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = inputEntity
            });
        }

        [FunctionName(nameof(DeleteInput))]
        public static async Task<IActionResult> DeleteInput(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "input/{id}")] HttpRequest req,
            [Table("input", "INPUT", "{id}", Connection = "AzureWebJobsStorage")] InputEntity inputEntity,
            [Table("input", Connection ="AzureWebJobsStorage")] CloudTable inputTable,
            string id,
            ILogger log)
        {
            log.LogInformation($"Delete input: {id}, received.");

            if (inputEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Input not found."
                });
            }

            await inputTable.ExecuteAsync(TableOperation.Delete(inputEntity));

            string message = $"Input: {inputEntity.RowKey}, deleted.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = inputEntity
            });
        }



    }
}
