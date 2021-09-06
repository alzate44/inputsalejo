using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using inputsalejo.Common.Models;
using inputsalejo.Functions.Entities;
using inputsalejo.Common.Responses;

namespace inputsalejo.Functions.Functions
{
    public static class ConsolidatedApi
    {
        [FunctionName(nameof(ConsolidatedInputs))]
        public static async Task<IActionResult> ConsolidatedInputs(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "consolidated")] HttpRequest req,
            [Table("consolidated", Connection = "AzureWebJobsStorage")] CloudTable consolidatedTable,
            ILogger log)
        {
            log.LogInformation("Recieved a new consolidated.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Consolidated consolidated = JsonConvert.DeserializeObject<Consolidated>(requestBody);

            //if (string.IsNullOrEmpty(input?.EmployeeId.ToString()))
            //{
            //    return new BadRequestObjectResult(new Response
            //    {
            //        IsSuccess = false,
            //        Message = "The field EmployeeId cannot be empty."
            //    });

            //}


            ConsolidatedEntity consolidatedEntity = new ConsolidatedEntity
            {
                EmployeeId = consolidated.EmployeeId,
                ConsolidatedDate = DateTime.UtcNow,
                ETag = "*",
                Minutes = consolidated.Minutes,
                PartitionKey = "TIMES",
                RowKey = Guid.NewGuid().ToString(),


            };

            TableOperation addOperation = TableOperation.Insert(consolidatedEntity);
            await consolidatedTable.ExecuteAsync(addOperation);

            string message = "New input stored in table.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = consolidatedEntity
            });
        }

    }
}
