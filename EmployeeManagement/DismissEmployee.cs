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
    public partial class DismissEmployee : Form
    {
        private int _employeeId;
        private FileHelper<List<Employee>> _fileHelper = new FileHelper<List<Employee>>(Program.FilePath);

        public DismissEmployee(int id = 0)
        {
            InitializeComponent();
            _employeeId = id;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var employees = _fileHelper.DeserializeFromFile();
            var employee = employees.FirstOrDefault(x => x.Id == _employeeId);
            dtpDismiss.MinDate = employee.DateOfEmployment;
            employee.IsDismissed = true;

            _fileHelper.SerializeToFile(employees);
            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
