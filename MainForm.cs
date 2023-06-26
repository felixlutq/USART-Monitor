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
using System.Collections.Concurrent;

namespace USART_Monitor
{
    public partial class MainForm : Form
    {
        private Cache cache;
        private ConcurrentBag<String> concurrentBag;
        private delegate void SafeCallDelegate(string text);
        

        public MainForm()
        {
            InitializeComponent();
            this.toolStripButtonConnect.Enabled = false;
            this.toolStripButtonDisconnect.Enabled = false;
            cache = new Cache();
            concurrentBag = new ConcurrentBag<string>();
            Thread thread = new Thread(new ThreadStart(this.portScan));
            thread.IsBackground = true;
            thread.Start();
            Thread thread2 = new Thread(new ThreadStart(this.saveAndPrintDataThreadSafe));
            thread2.IsBackground = true;
            thread2.Start();
        }

        private void portScan()
        {
            while (true)
            {
                cache.availableSerialPortNames.Clear();
                cache.availableSerialPortNames.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                Thread.Sleep(1200);
            }
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this;
            SettingsForm settingsForm = new SettingsForm(ref this.cache);
            settingsForm.StartPosition = FormStartPosition.Manual;
            settingsForm.Location = new Point(this.Location.X + this.Width / 2 - settingsForm.Width / 2, this.Location.Y + 30);
            if (settingsForm.ShowDialog() == DialogResult.Yes)
            {
                Console.WriteLine("new settings available.");
            }
        }

        private void toolStripButtonPorts_Click(object sender, EventArgs e)
        {
            MainForm mainForm = this;
            PortsForm portsForm = new PortsForm(ref this.cache, ref mainForm);
            portsForm.StartPosition = FormStartPosition.Manual;
            portsForm.Location = new Point(this.Location.X + this.Width / 2 - portsForm.Width / 2, this.Location.Y + 30);
            if (portsForm.ShowDialog() == DialogResult.Yes)
            {
                toolStripButtonConnect.Enabled = true;
                toolStripButtonDisconnect.Enabled = false;
            }
        }

        private void serialPort1_pinChanged(object sender, System.IO.Ports.SerialPinChangedEventArgs e)
        {
            Console.WriteLine("pin changed.");
        }

        private void serialPort1_errorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("error.");
        }

        private void saveAndPrintDataThreadSafe()
        {
            while (true)
            {
                String text;
                if (concurrentBag.TryTake(out text))
                {
                    writeTextSafe(text);
                }
                else
                {
                    Thread.Sleep(100);
                }

            }
        }

        private void processDataThreadSafe(System.IO.Ports.SerialPort serialPort)
        {
            StringBuilder newLine = new StringBuilder(1024);
            if (this.cache.bShowDateTime)
            {
                newLine.Append(DateTime.Now.ToString(this.cache.dateTimeFormat));
                newLine.Append("  ");
            }
            if (this.cache.selectedPortNames.Count > 1)
            {
                newLine.Append(serialPort.PortName);
                newLine.Append("  ");
            }
            if (this.cache.bShowRXTX)
            {
                newLine.Append("RX  ");
            }
            if (newLine.Length > 0)
            {
                newLine.Append("    ");
            }
            newLine.Append(serialPort.ReadLine());
            concurrentBag.Add(newLine.ToString());
        }

        private void serialPort1_dataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            processDataThreadSafe(this.serialPort1);
        }
        private void serialPort2_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            processDataThreadSafe(this.serialPort2);
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
            if (this.cache.selectedPortNames.Count > 0)
            {
                this.serialPort1.PortName = this.cache.selectedPortNames[0];
                if (this.serialPort1.IsOpen == false)
                {
                    this.serialPort1.Open();
                }
            }
            if (this.cache.selectedPortNames.Count > 1)
            {
                this.serialPort2.PortName = this.cache.selectedPortNames[1];
                if (this.serialPort2.IsOpen == false)
                {
                    this.serialPort2.Open();
                }
            }
            if (this.serialPort1.IsOpen || this.serialPort2.IsOpen)
            {
                this.toolStripButtonConnect.Enabled = false;
                this.toolStripButtonDisconnect.Enabled = true;
                this.cache.bConnected = true;
            }
        }

        private void toolStripButtonDisconnect_Click(object sender, EventArgs e)
        {
            if (this.serialPort1.IsOpen)
            {
                this.serialPort1.Close(); // TODO: exception throws when serialPort1_dataReceived not return
            }
            if (this.serialPort2.IsOpen)
            {
                this.serialPort2.Close();
            }
            this.toolStripButtonConnect.Enabled = true;
            this.toolStripButtonDisconnect.Enabled = false;
            this.cache.bConnected = false;
        }

        private void autoScrollDown(object sender, EventArgs e)
        {
            this.textBox1.SelectionStart = textBox1.TextLength;
            this.textBox1.ScrollToCaret();
        }

    }
}
