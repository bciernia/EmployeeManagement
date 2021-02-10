using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
        private int _employeeId;

        private FileHelper<List<Employee>> _fileHelper = new FileHelper<List<Employee>>(Program.FilePath);

        public AddEditEmployee(int id = 0)
        {
            InitializeComponent();
            _employeeId = id;
            GetEmployeeData();
            tbName.Select();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var employees = _fileHelper.DeserializeFromFile();

            if (_employeeId != 0)
            {
                employees.RemoveAll(x => x.Id == _employeeId);
            }
            else
            {
                var employeeWithHighestId = employees.OrderByDescending(x => x.Id).FirstOrDefault();
                _employeeId = employeeWithHighestId == null ? 1 : employeeWithHighestId.Id + 1;
            }

            var employee = new Employee
            {
                Id = _employeeId,
                FirstName = tbName.Text,
                LastName = tbLastName.Text,
                DateOfEmployment = dtpEmployment.Value.Date,
                Salary = Decimal.Parse(tbSalary.Text),
                Comments = rtbComments.Text,
            };
            employees.Add(employee);
            _fileHelper.SerializeToFile(employees);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GetEmployeeData()
        {
            if (_employeeId != 0)
            {
                var employees = _fileHelper.DeserializeFromFile();
                var employee = employees.FirstOrDefault(x => x.Id == _employeeId);
                //DateTime dt = DateTime.Parse(employee.DateOfEmployment);

                if (employee == null)
                    throw new Exception("There is no employee with this id");

                tbId.Text = employee.Id.ToString();
                tbName.Text = employee.FirstName;
                tbLastName.Text = employee.LastName;
                dtpEmployment.Value = employee.DateOfEmployment.Date;
                tbSalary.Text = employee.Salary.ToString();
                rtbComments.Text = employee.Comments;
            }

            Console.WriteLine("");

        }
    }
}
