using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace inputsalejo.Functions.Entities
{
    public class InputEntity : TableEntity
    {

        public int EmployeeId { get; set; }
        public DateTime InputDate { get; set; }
        public int Type { get; set; }
        public bool Consolidated { get; set; }
    }
}
