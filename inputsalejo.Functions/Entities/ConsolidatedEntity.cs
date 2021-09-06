using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace inputsalejo.Functions.Entities
{
    public class ConsolidatedEntity : TableEntity
    {
        public int EmployeeId { get; set; }
        public DateTime ConsolidatedDate { get; set; }
        public int Minutes { get; set; }
    }
}
