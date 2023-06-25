using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace USART_Monitor
{
    public partial class MainForm : Form
    {
        private Cache cache;
        private delegate void SafeCallDelegate(string text);

        public MainForm()
        {
            InitializeComponent();
            //this.toolStripButtonConnect.Enabled = false;
            //this.toolStripButtonDisconnect.Enabled = false;
            cache = new Cache();
            Thread thread = new Thread(new ThreadStart(this.portScan));
            thread.IsBackground = true;
            thread.Start();
            
            
        }

        private void portScan()
        {
            while (true)
            {
                cache.availableSerialPortNames = System.IO.Ports.SerialPort.GetPortNames();
                Thread.Sleep(1200);
            }
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonPorts_Click(object sender, EventArgs e)
        {
            PortsForm portsForm = new PortsForm(this.cache);
            portsForm.Show();
            portsForm.SetDesktopLocation(this.Location.X, this.Location.Y);
            
            
        }

        private void serialPort1_pinChanged(object sender, System.IO.Ports.SerialPinChangedEventArgs e)
        {
            Console.WriteLine("pin changed.");
        }

        private void serialPort1_errorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("error.");
        }

        private void serialPort1_dataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            String newLine = this.serialPort1.ReadLine();
            Console.WriteLine(newLine);
            writeTextSafe(newLine);
        }

        private void writeTextSafe(string text)
        {
            if (textBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(writeTextSafe);
                textBox1.Invoke(d, new object[] { text });
            }
            else
            {
                textBox1.Text = textBox1.Text + "\n" + text;
            }
        }

        private void toolStripButtonConnect_Click(object sender, EventArgs e)
        {

            this.serialPort1.PortName = this.cache.selectedPortNames[0];
            if (this.serialPort1.IsOpen == false)
            {
                this.serialPort1.Open();
            }
            if (this.serialPort1.IsOpen)
            {
                this.toolStripButtonConnect.Enabled = false;
                this.toolStripButtonDisconnect.Enabled = true;
            }

        }

        private void toolStripButtonDisconnect_Click(object sender, EventArgs e)
        {
            if (this.serialPort1.IsOpen)
            {
                this.toolStripButtonConnect.Enabled = true;
                this.toolStripButtonDisconnect.Enabled = false;
                this.serialPort1.Close();
            }
        }

        private void autoScrollDown(object sender, EventArgs e)
        {
            this.textBox1.SelectionStart = textBox1.TextLength;
            this.textBox1.ScrollToCaret();
        }
    }
}
