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

        public PortsForm(Cache cache)
        {
            InitializeComponent();
            this.cache = cache;
            ListPorts();
        }

        private void ListPorts()
        {
            foreach(String name in this.cache.availableSerialPortNames)
            {
                checkedListBoxPortsList.Items.Add(name);
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            Array.Clear(this.cache.selectedPortNames, 0, this.cache.selectedPortNames.Length);
            
            for(int i = 0; i < this.checkedListBoxPortsList.CheckedItems.Count; i ++) {
                this.cache.selectedPortNames[i] = this.checkedListBoxPortsList.CheckedItems[i].ToString();
            }
            Console.WriteLine("selectedPortNames Length: " + this.cache.selectedPortNames.Length);
            foreach(var it in this.cache.selectedPortNames)
            {
                Console.WriteLine(it);
            }
            
            
        }
    }
}
