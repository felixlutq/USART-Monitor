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
using System.IO;
using System.Security.Permissions;

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
            initialiseComponents();
            cache = new Cache();
            concurrentBag = new ConcurrentBag<string>();
            Thread thread = new Thread(new ThreadStart(this.portScan));
            thread.IsBackground = true;
            thread.Start();
            Thread thread2 = new Thread(new ThreadStart(this.saveAndPrintDataThreadSafe));
            thread2.IsBackground = true;
            thread2.Start();
        }

        private void initialiseComponents()
        {
            clearSerialPortNames();
            this.toolStripButtonConnect.Enabled = false;
            this.toolStripButtonDisconnect.Enabled = false;
            this.buttonSend.Enabled = false;
            comboBoxBaudrate.SelectedItem = "9600";
            comboBoxDataType.SelectedItem = "text";
        }

        private void clearSerialPortNames()
        {
            this.serialPort1.PortName = "None";
            this.serialPort2.PortName = "None";
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
                //TODO: show new settings applied succeed on status bar or somewhere else.
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
                clearSerialPortNames();
                comboBoxSendTo.Items.Clear();
                comboBoxSendTo.Text = "";
                if (this.cache.selectedPortNames.Count > 0)
                {
                    this.serialPort1.PortName = this.cache.selectedPortNames[0];
                    comboBoxSendTo.Items.Add(this.cache.selectedPortNames[0]);
                    comboBoxSendTo.Text = this.cache.selectedPortNames[0];
                }
                if (this.cache.selectedPortNames.Count > 1)
                {
                    this.serialPort2.PortName = this.cache.selectedPortNames[1];
                    comboBoxSendTo.Items.Add(this.cache.selectedPortNames[1]);
                    comboBoxSendTo.Items.Add("Both");
                    comboBoxSendTo.Text = "Both";
                }
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
                    text += "\r\n"; // TODO: when baudrate not match and receives strange characters, \r\n not always work.
                    writeTextSafe(text);
                    appendTextToFile(this.cache.logFileName, text);
                }
                else
                {
                    Thread.Sleep(100);
                }

            }
        }

        private void appendTextToFile(String fileName, String text)
        {
            FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Write);
            byte[] bytes = new UTF8Encoding(true).GetBytes(text);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
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
            String str = "";

            lock (serialPort)
            {
                if (serialPort.IsOpen)
                {
                    try
                    {
                        str = serialPort.ReadLine();
                    } catch (TimeoutException e)
                    {
                        try
                        {
                            str = serialPort.ReadExisting();
                        } catch (Exception e2)
                        {
                            return;
                        }
                    } catch (Exception e)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            newLine.Append(str);
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
                textBox1.Text = textBox1.Text + text;
            }
        }

        private void toolStripButtonConnect_Click(object sender, EventArgs e)
        {
            if (this.cache.selectedPortNames.Contains(this.serialPort1.PortName))
            {
                if (this.serialPort1.IsOpen == false)
                {
                    this.serialPort1.BaudRate = this.cache.baudRate;
                    this.serialPort1.ReadTimeout = this.cache.readTimeoutMilliseconds;
                    this.serialPort1.Open();
                }
            }
            if (this.cache.selectedPortNames.Contains(this.serialPort2.PortName))
            {
                if (this.serialPort2.IsOpen == false)
                {
                    this.serialPort2.BaudRate = this.cache.baudRate;
                    this.serialPort2.ReadTimeout = this.cache.readTimeoutMilliseconds;
                    this.serialPort2.Open();
                }
            }

            if (this.serialPort1.IsOpen || this.serialPort2.IsOpen)
            {
                this.toolStripButtonConnect.Enabled = false;
                this.toolStripButtonDisconnect.Enabled = true;
                this.buttonSend.Enabled = true;
                this.cache.bConnected = true;
            }
        }

        private void toolStripButtonDisconnect_Click(object sender, EventArgs e)
        {
            if (this.serialPort1.IsOpen)
            {
                this.serialPort1.Dispose();
                this.serialPort1.Close();
            }
            if (this.serialPort2.IsOpen)
            {
                this.serialPort2.Dispose();
                this.serialPort2.Close();
            }
            this.toolStripButtonConnect.Enabled = true;
            this.toolStripButtonDisconnect.Enabled = false;
            this.buttonSend.Enabled = false;
            this.cache.bConnected = false;
        }

        private void autoScrollDown(object sender, EventArgs e)
        {
            this.textBox1.SelectionStart = textBox1.TextLength;
            this.textBox1.ScrollToCaret();
        }

        private bool sendInputToSerialPort(System.IO.Ports.SerialPort port)
        {
            byte[] bytes = null;
            if (comboBoxDataType.Text.Contains("text"))
            {
                bytes = new UTF8Encoding(true).GetBytes(textBoxInput.Text);
            }
            else if (comboBoxDataType.Text.Contains("int"))
            {
                int intValue = 0;
                try
                {
                    intValue = int.Parse(textBoxInput.Text);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); // TODO: show warning or help message
                    return false;
                }
                bytes = BitConverter.GetBytes(intValue);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }
            }
            else if (comboBoxDataType.Text.Contains("short"))
            {
                short shortValue = 0;
                try
                {
                    shortValue = short.Parse(textBoxInput.Text);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); // TODO: show warning or help message
                    return false;
                }
                bytes = BitConverter.GetBytes(shortValue);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }
            }
            else if (comboBoxDataType.Text.Contains("byte"))
            {
                byte byteValue = 0;
                try
                {
                    byteValue = byte.Parse(textBoxInput.Text);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); // TODO: show warning or help message
                    return false;
                }
                bytes = new byte[1];
                bytes[0] = byteValue;
            }
            else if (comboBoxDataType.Text.Contains("double"))
            {
                double doubleValue = 0;
                try
                {
                    doubleValue = double.Parse(textBoxInput.Text);
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message); // TODO: show warning or help message
                    return false;
                }
                bytes = BitConverter.GetBytes(doubleValue);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }
            }
            else if (comboBoxDataType.Text.Contains("float"))
            {
                float floatValue = 0;
                try
                {
                    floatValue = float.Parse(textBoxInput.Text);
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message); // TODO: show warning or help message
                    return false;
                }
                bytes = BitConverter.GetBytes(floatValue);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }
            } else if (comboBoxDataType.Text.Contains("long"))
            {
                long longValue = 0;
                try
                {
                    longValue = long.Parse(textBoxInput.Text);
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }
            }

            if (bytes == null)
            {
                return false;
            }

            if (port.IsOpen)
            {
                port.Write(bytes, 0, bytes.Length);
                return true;
            }
            return false;
        }

        private bool verifyUserInput()
        {
            // TODO: check int, short, byte, double, float
            return true;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            String inputString = textBoxInput.Text;
            if (verifyUserInput() == false)
            {
                // TODO: show warning or help information
                return;
            }
            if (inputString.Length == 0)
            {
                return;
            }
            bool bSendToBothPorts = this.comboBoxSendTo.Text.Contains("Both");
            bool bSendSucceed = false;
            if (bSendToBothPorts || this.comboBoxSendTo.Text.Contains(this.serialPort1.PortName))
            {
                bSendSucceed = sendInputToSerialPort(this.serialPort1);
            }
            if (bSendToBothPorts || this.comboBoxSendTo.Text.Contains(this.serialPort2.PortName))
            {
                bSendSucceed = sendInputToSerialPort(this.serialPort2);
            }
            Console.WriteLine(bSendSucceed);
            if (bSendSucceed)
            {
                textBoxInput.Clear();
            }
        }

        private void comboBoxBaudrate_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int baudRate = int.Parse(comboBoxBaudrate.Text);
            this.cache.baudRate = baudRate;
            if (this.serialPort1.IsOpen)
            {
                this.serialPort1.BaudRate = baudRate;
            }
            if (this.serialPort2.IsOpen)
            {
                this.serialPort2.BaudRate = baudRate;
            }
        }
    }
}
