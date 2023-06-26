using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            this.cache.bShowDateTime = this.checkBoxDateTime.Checked;
            this.cache.logFileName = textBoxLogFileName.Text;
            this.cache.bShowRXTX = this.checkBoxShowRXTX.Checked;
            this.DialogResult = DialogResult.Yes;
        }

        private void Browse_Click(object sender, EventArgs e)
        {

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
