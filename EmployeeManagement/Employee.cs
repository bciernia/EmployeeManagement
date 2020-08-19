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
        public string DateOfEmployment { get; set; }
        public int Salary { get; set; }
        public string Comments { get; set; }
        public string DateOfDismiss { get; set; }
    }

    public class Group
    { 
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
