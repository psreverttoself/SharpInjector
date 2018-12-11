using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace SharpInjector
{
    public partial class PrimaryForm : Form
    {
        private Process[] Processes;
        private Injector InjectorInstance;

        public PrimaryForm()
        {
            InitializeComponent();
            InjectorInstance = new Injector();
        }

        private static string GetWhiteSpace(string input, int endLength)
        {
            string result = input;

            int iterations = endLength - input.Trim().Length;

            for (int i = 0; i < iterations; i++)
            {
                result += "0";
            }

            return result;
        }

        private void ProcessSelectComboBox_DropDown(object sender, EventArgs e)
        {
            Processes = Process.GetProcesses();

            ProcessSelectComboBox.Items.Clear();

            for (int i = 0; i < Processes.Length; i++)
            {
                if (PidCheckBox.Checked)
                {
                    ProcessSelectComboBox.Items.Add("PID: " + Processes[i].Id.ToString());
                }
                else
                {
                    ProcessSelectComboBox.Items.Add(Processes[i].ProcessName);
                }
            }
        }

        private void InjectButton_Click(object sender, EventArgs e)
        {
            string dllPath = SelectedDllTextBox.Text;

            int pid = -1;

            if (ProcessSelectComboBox.SelectedIndex >= 0 && ProcessSelectComboBox.SelectedIndex < Processes.Length)
            {
                pid = Processes[ProcessSelectComboBox.SelectedIndex].Id;
            }

            if (File.Exists(dllPath) && pid != -1)
            {
                if (InjectorInstance.Inject(dllPath, pid))
                {
                    MessageBox.Show("DLL injected successfully!", "Success");
                }
            }
        }

        private void SelectDllButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectDialog = new OpenFileDialog();

            selectDialog.Title = "Select DLL";
            selectDialog.Multiselect = false;
            selectDialog.Filter = "DLL (*.dll)|*.dll";

            if (selectDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedDllTextBox.Text = selectDialog.FileName;
            }
        }
    }
}
