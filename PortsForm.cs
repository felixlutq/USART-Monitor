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
    public partial class PortsForm : Form
    {
        private Cache cache;
        private MainForm mainForm;

        public PortsForm(ref Cache cache, ref MainForm mainForm)
        {
            InitializeComponent();
            this.cache = cache;
            this.mainForm = mainForm;
            updatePortList();
            this.applyButton.Enabled = false;
        }

        private void updatePortList()
        {
            foreach(String name in this.cache.availableSerialPortNames)
            {
                checkedListBoxPortsList.Items.Add(name, this.cache.selectedPortNames.Contains(name));
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if (this.cache.bConnected)
            {
                MessageBox.Show("Disconnect before change ports.");
                return;
            }
            if (this.checkedListBoxPortsList.CheckedItems.Count == 0)
            {
                MessageBox.Show("No port selected.");
                return;
            }
            if (this.checkedListBoxPortsList.CheckedItems.Count > Cache.maxSuportPortsCount)
            {
                MessageBox.Show("Too many ports selected, maximum is " + Cache.maxSuportPortsCount);
                return;
            }

            bool bSelectionChanged = false;
            var names = new List<String>(this.checkedListBoxPortsList.CheckedItems.Count);
            for (var i = 0; i < this.checkedListBoxPortsList.CheckedItems.Count; i ++)
            {
                names.Add(checkedListBoxPortsList.CheckedItems[i].ToString());
            }

            if (names.Count != this.cache.selectedPortNames.Count)
            {
                bSelectionChanged = true;
            } else
            {
                names.Sort();
                this.cache.selectedPortNames.Sort();
                for (int i = 0; i < this.cache.selectedPortNames.Count; i ++)
                {
                    if (names[i].Equals(this.cache.selectedPortNames[i]) == false)
                    {
                        bSelectionChanged = true;
                        break;
                    }
                }
            }

            this.applyButton.Enabled = false;

            if (bSelectionChanged)
            {
                this.cache.selectedPortNames.Clear();
                this.cache.selectedPortNames.AddRange(names);
                this.DialogResult = DialogResult.Yes;
            } else
            {
                this.DialogResult = DialogResult.No;
            }
        }

        private void checkedListBoxPortsList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.applyButton.Enabled = true;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
