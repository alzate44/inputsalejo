using System;
using System.Collections.Generic;
using System.Text;

namespace inputsalejo.Common.Models
{
    public class Input
    {
        public int EmployeeId { get; set; }
        public DateTime InputDate { get; set; }
        public int Type { get; set; }
        public bool Consolidated { get; set; }
    }
}
