using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace EmployeeManagement
{
    public partial class Main : Form
    {
        private FileHelper<List<Employee>> _fileHelper = new FileHelper<List<Employee>>(Program.FilePath);

        public Main()
        {
            InitializeComponent();

            var employees = _fileHelper.DeserializeFromFile();
            dgvData.DataSource = employees;
            RefreshData();
            SetColumnsHeader();
            InitComboboxGroups();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addEditEmployee = new AddEditEmployee();
            addEditEmployee.FormClosing += AddEditEmployee_FormClosing;
            addEditEmployee.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please, select employee to edit");
                return;
            }

            if (dgvData.SelectedRows[0].Cells[6].Value == null)
            {
                MessageBox.Show("Employee has been dismissed");
                return;
            }
            var addEditEmployee = new AddEditEmployee(
                Convert.ToInt32(dgvData.SelectedRows[0].Cells[0].Value));

            addEditEmployee.FormClosing += AddEditEmployee_FormClosing;
            addEditEmployee.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("Employee is not selected");
                return;
            }

            if (dgvData.SelectedRows[0].Cells[6].Value == null)
            {
                MessageBox.Show("Employee has been dismised");
                return;
            }
            var selectedEmployee = dgvData.SelectedRows[0];
            var confirmDelete =
                MessageBox.Show($"Are you sure, to delete {selectedEmployee.Cells[1].Value.ToString().Trim()}?",
                "Delete employee", MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                var dismissEmployee = new DismissEmployee(Convert.ToInt32(dgvData.SelectedRows[0].Cells[0].Value));
                dismissEmployee.FormClosing += DismissEmployee_FormClosing;
                dismissEmployee.ShowDialog();
            }
        }     

        private void SetColumnsHeader()
        {
            dgvData.Columns[0].HeaderText = "ID";
            dgvData.Columns[1].HeaderText = "First name";
            dgvData.Columns[2].HeaderText = "Last name";
            dgvData.Columns[3].HeaderText = "Employment";
            dgvData.Columns[4].HeaderText = "Salary";
            dgvData.Columns[5].HeaderText = "Comments";
            dgvData.Columns[6].HeaderText = "Dismissal";
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void cbEmployeeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvData.CurrentCell = null;

            if (cbEmployeeList.SelectedIndex == 0)
            {
                RefreshData();
            }
            else if (cbEmployeeList.SelectedIndex == 1)
            {
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    if (Convert.ToDecimal(dgvData.Rows[i].Cells[4].Value) < 5000 && dgvData.Rows[i].Cells[6].Value.ToString() == "01.01.0001 00:00:00")
                        dgvData.Rows[i].Visible = true;
                    else
                        dgvData.Rows[i].Visible = false;
                }
            } //&& dgvData.Rows[i].Cells[6].Value == null
            else if (cbEmployeeList.SelectedIndex == 2)
            {
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    if (Convert.ToDecimal(dgvData.Rows[i].Cells[4].Value) >= 5000 && dgvData.Rows[i].Cells[6].Value.ToString() == "01.01.0001 00:00:00")
                        dgvData.Rows[i].Visible = true;
                    else
                        dgvData.Rows[i].Visible = false;
                }
            }
            else if (cbEmployeeList.SelectedIndex == 3)
            {
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    if (dgvData.Rows[i].Cells[6].Value.ToString() != "01.01.0001 00:00:00")
                        dgvData.Rows[i].Visible = false;
                    else
                        dgvData.Rows[i].Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    if (dgvData.Rows[i].Cells[6].Value.ToString() != "01.01.0001 00:00:00")
                        dgvData.Rows[i].Visible = true;
                    else
                        dgvData.Rows[i].Visible = false;
                }
            }
        }

        private void InitComboboxGroups()
        {
            var _groups = new List<Group>
            {
                new Group { Id = 0, Name = "All employees" },
                new Group { Id = 1, Name = "Earnings below 5000" },
                new Group { Id = 2, Name = "Earnings over 5000" },
                new Group { Id = 3, Name = "Hired employees" },
                new Group { Id = 4, Name = "Redundant workers" },
            };
            cbEmployeeList.DataSource = _groups;
            cbEmployeeList.ValueMember = "Id";
            cbEmployeeList.DisplayMember = "Name";
        }

        private void RefreshData()
        {
            var employees = _fileHelper.DeserializeFromFile();
            employees = employees.OrderBy(x => x.Id).ToList();
            dgvData.DataSource = employees;
        }

        private void restoreEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("Employee is not selected");
                return;
            }

            if (dgvData.SelectedRows[0].Cells[6].Value == null)
            {
                MessageBox.Show("Employee has not been dismissed");
                return;
            }

            var selectedEmployee = dgvData.SelectedRows[0];
            var confirmDelete =
                MessageBox.Show($"Are you sure, to restore that person?", "Restore employee", MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                //Przywracanie pracowników TODO

                int _employeeId = (int)selectedEmployee.Cells[0].Value;

                selectedEmployee.Cells[3].Value = DateTime.Now.Date;
                selectedEmployee.Cells[6].Value = null;

                var employees = _fileHelper.DeserializeFromFile();
                var employee = employees.FirstOrDefault(x => x.Id == _employeeId);
                employee.DateOfEmployment = DateTime.Now.Date;
                employee.DateOfDismiss = DateTime.Now.AddDays(2);

                _fileHelper.SerializeToFile(employees);

                MessageBox.Show("Welcome in our company!");

            }
        }

        private void AddEditEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshData();
        }

        private void DismissEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshData();
        }
        private void RestoreEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshData();
        }
    }
}