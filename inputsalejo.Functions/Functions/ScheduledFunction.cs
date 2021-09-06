using System;
using System.Threading.Tasks;
using inputsalejo.Functions.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace inputsalejo.Functions.Functions
{
    public static class ScheduledFunction
    {
        [FunctionName("ScheduledFunction")]
        public static async Task Run(
            [TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
            [Table("input", Connection = "AzureWebJobsStorage")] CloudTable inputTable,
            ILogger log)
        {
            log.LogInformation($"Consolidated information: {DateTime.Now}");
            string filter = TableQuery.GenerateFilterConditionForBool("Consolidated", QueryComparisons.Equal, true);
            TableQuery<InputEntity> query = new TableQuery<InputEntity>().Where(filter);
            TableQuerySegment<InputEntity> consolidatedInputs = await inputTable.ExecuteQuerySegmentedAsync(query, null);
            int consolidated = 0;

            foreach (InputEntity consolidatedInput in consolidatedInputs)
            {
               
            }

        }
    }
}
