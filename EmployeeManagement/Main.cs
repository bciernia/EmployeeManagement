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
    public partial class Main : Form
    {
        private string _fileEmployeesPath = Path.Combine(Environment.CurrentDirectory, "employees.txt");

        public Main()
        {
            InitializeComponent();

            var employees = DeserializeFromFile();

            dgvData.DataSource = employees;
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

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addEditEmployee = new AddEditEmployee();
            addEditEmployee.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please, select employee to edit");
                return;
            }

            var addEditEmployee = new AddEditEmployee(
                Convert.ToInt32(dgvData.SelectedRows[0].Cells[0].Value));
            addEditEmployee.ShowDialog();


        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var employees = DeserializeFromFile();

            dgvData.DataSource = employees;
        }
    }
}
