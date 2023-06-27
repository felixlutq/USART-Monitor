using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USART_Monitor
{
    public partial class SettingsForm : Form
    {
        private Cache cache;
        public SettingsForm(ref Cache cache)
        {
            InitializeComponent();
            this.cache = cache;
            checkBoxDateTime.Checked = this.cache.bShowDateTime;
            checkBoxShowRXTX.Checked = this.cache.bShowRXTX;
            this.textBoxLogFileName.Text = this.cache.logFileName;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            this.cache.logFileName = this.textBoxLogFileName.Text;

            if (System.IO.Path.IsPathRooted(this.cache.logFileName) == false) {
                MessageBox.Show("Absolute path information is required.");
                return;
            }

            var fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Write,
                                System.Security.AccessControl.AccessControlActions.View,
                                this.cache.logFileName);

            if (fileIOPermission.AllFiles == FileIOPermissionAccess.Write)
            {
                MessageBox.Show("Have no permission to access " + this.cache.logFileName + ", please specify other path.");
                return;
            }

            this.cache.bShowDateTime = this.checkBoxDateTime.Checked;
            this.cache.logFileName = textBoxLogFileName.Text;
            this.cache.bShowRXTX = this.checkBoxShowRXTX.Checked;
            this.DialogResult = DialogResult.Yes;
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            //System.IO.Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBoxLogFileName.Text = saveFileDialog1.FileName;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
