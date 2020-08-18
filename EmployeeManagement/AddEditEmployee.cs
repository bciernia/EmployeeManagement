using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace EmployeeManagement
{
    public partial class AddEditEmployee : Form
    {

        private string _fileEmployeesPath = Path.Combine(Environment.CurrentDirectory, "employees.txt");

        public AddEditEmployee(int id = 0)
        {
            InitializeComponent();

        }

        public void SerializeToFile(List<Employee> employees)
        {
            var serializer = new XmlSerializer(typeof(List<Employee>));


            using (var streamWriter = new StreamWriter(_fileEmployeesPath))
            {
                serializer.Serialize(streamWriter, employees);
                streamWriter.Close();
            }
        }

        public List<Employee> DeserializeFromFile()
        {
            if (!File.Exists(_fileEmployeesPath))
                return new List<Employee>();

            var serializer = new XmlSerializer(typeof(List<Employee>));

            using (var streamReader = new StreamReader(_fileEmployeesPath))
            {
                var employees = (List<Employee>)serializer.Deserialize(streamReader);
                streamReader.Close();
                return employees;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var employees = DeserializeFromFile();

            var employeeWithHighestId = employees.OrderByDescending(x => x.Id).FirstOrDefault();
            int salary = 0;
            var employeeId = employeeWithHighestId == null ? 1 : employeeWithHighestId.Id + 1;
                
            salary = Convert.ToInt32(tbSalary.Text);

            var employee = new Employee
            {
                Id = employeeId,
                FirstName = tbName.Text,
                LastName = tbLastName.Text,
                DateOfEmployment = dtpEmployment.Value.ToString(),
                Salary = salary,
                Comments = rtbComments.Text,
            };

            employees.Add(employee);

            SerializeToFile(employees);

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
