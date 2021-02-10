using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfEmployment { get; set; }
        public decimal Salary { get; set; }
        public string Comments { get; set; }
        public DateTime DateOfDismiss { get; set; }
        public bool IsDismissed { get; set; }
    }
}
