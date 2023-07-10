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
using System.Management;

namespace USART_Monitor
{
    public partial class MainForm : Form
    {
        private Size mainFormInitializeSize;
        private Size textBoxOutputInitializeSize;
        private Size textBoxInputInitializeSize;
        private Point textBoxInputInitializeLocation;
        private Point panel1IntializedLocation;
        private Point panel2IntializedLocation;
        private Cache cache;
        private ConcurrentBag<String> concurrentBag;
        private delegate void writeTextSafeDelegate(string text);
        private delegate void updatePortsStatusOnStatusBarDelegate();
        private PortsForm portsForm;
        private Thread printThread;
        private volatile bool printThreadContinueRunning;
        private volatile bool bHasWorkToDo;

        public MainForm()
        {
            InitializeComponent();
            initialiseComponents();
            cache = new Cache();
            cache.availableSerialPortNames.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            concurrentBag = new ConcurrentBag<string>();

            printThread = null;
            printThreadContinueRunning = false;
            bHasWorkToDo = false;

            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
            ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += new EventArrivedEventHandler(OnDeviceInserted);
            insertWatcher.Start();

            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");
            ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += new EventArrivedEventHandler(OnDeviceRemoved);
            removeWatcher.Start();
        }

        private void initialiseComponents()
        {
            mainFormInitializeSize = this.Size;
            textBoxOutputInitializeSize = this.textBoxOutput.Size;
            textBoxInputInitializeSize = this.textBoxInput.Size;
            textBoxInputInitializeLocation = this.textBoxInput.Location;
            panel1IntializedLocation = this.panel1.Location;
            panel2IntializedLocation = this.panel2.Location;

            clearSerialPortNames();
            this.portsForm = null;
            this.toolStripButtonConnect.Enabled = false;
            this.toolStripButtonDisconnect.Enabled = false;
            this.buttonSend.Enabled = false;
            comboBoxBaudrate.SelectedItem = "9600";
            comboBoxDataType.SelectedItem = "text";
            toolStripStatusLabelTime.Alignment = ToolStripItemAlignment.Right;
        }

        private void OnDeviceInserted(object sender, EventArrivedEventArgs e)
        {
            // TODO: how to get device info from the event
            // TODO: 5 events are tiggered, how to just trigger the last one?
            //getAllSerialPortsInfo();
            String []portNames = System.IO.Ports.SerialPort.GetPortNames();
            foreach(var name in portNames)
            {
                if (this.cache.availableSerialPortNames.Contains(name) == false)
                {
                    this.cache.availableSerialPortNames.Add(name);
                    if (this.portsForm != null)
                    {
                        this.portsForm.addPortName(name);
                    }
                    if (this.serialPort1.PortName.Equals(name))
                    {
                        this.serialPort1.Open();
                        updatePortsStatusOnStatusBar();
                    }
                    else if (this.serialPort2.PortName.Equals(name))
                    {
                        this.serialPort2.Open();
                        updatePortsStatusOnStatusBar();
                    }
                }
            }
        }

        private void OnDeviceRemoved(object sender, EventArrivedEventArgs e)
        {
            //getAllSerialPortsInfo();
            String[] portNames = System.IO.Ports.SerialPort.GetPortNames();
            String[] cachedPortNames = this.cache.availableSerialPortNames.ToArray();
            foreach (var name in cachedPortNames)
            {
                if (portNames.Contains(name) == false)
                {
                    this.cache.availableSerialPortNames.Remove(name);
                    if (this.portsForm != null)
                    {
                        this.portsForm.removePortName(name);
                        updatePortsStatusOnStatusBar();
                    }
                }
            }
        }

        // name=Communications Port (COM1),des=Communications Port
        // name=USB Serial Port (COM3),des=USB Serial Port
        private Dictionary<string, string> getAllSerialPortsInfo()
        {
            var dict = new Dictionary<string, string>();
            using (var mos = new ManagementObjectSearcher(@"Select * From Win32_PNPEntity"))
            {
                using (ManagementObjectCollection collection = mos.Get())
                {
                    foreach (var device in collection)
                    {
                        object value = device.GetPropertyValue("Name");
                        if (null != value)
                        {
                            var name = device.GetPropertyValue("Name").ToString();
                            string comName;
                            string description;
                            if (name.StartsWith("Communications Port (COM"))
                            {
                                int offset = name.IndexOf('(') + 1;
                                comName = name.Substring(offset, name.IndexOf(')') - offset);
                                description = "";
                            }
                            else if (name.StartsWith("USB Serial Port (COM"))
                            {
                                int offset = name.IndexOf('(') + 1;
                                comName = comName = name.Substring(offset, name.IndexOf(')') - offset);
                                description = device.GetPropertyValue("Description").ToString();
                            }
                            else
                            {
                                continue;
                            }
                            dict.Add(comName, description);
                            Console.WriteLine("name=" + comName + ",des=" + description);
                        }
                    }
                }
            }
            return dict;
        }

        private void clearSerialPortNames()
        {
            this.serialPort1.PortName = "None";
            this.serialPort2.PortName = "None";
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
            if (this.portsForm == null)
            {
                portsForm = new PortsForm(ref this.cache, ref mainForm);
                foreach (string name in this.cache.availableSerialPortNames)
                {
                    portsForm.addPortName(name);
                }
            }

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
            Console.WriteLine("serialPort1_pinChanged");
        }

        private void serialPort1_errorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("serialPort1_errorReceived");
        }

        private void saveAndPrintDataThreadSafe()
        {
            while (printThreadContinueRunning)
            {
                if (bHasWorkToDo)
                {
                    String text;
                    while (concurrentBag.TryTake(out text))
                    {
                        writeTextSafe(text);
                        appendTextToFile(this.cache.logFileName, text);
                    }
                    Thread.Sleep(100);
                }
                else
                {
                    Thread.Yield();
                }
            }
        }

        private void appendTextToFile(String fileName, String text)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Write);
                byte[] bytes = new UTF8Encoding(true).GetBytes(text + "\r\n");
                fs.Write(bytes, 0, bytes.Length);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (null != fs)
                {
                    fs.Close();
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
            String str = "";

            lock (serialPort)
            {
                if (serialPort.IsOpen)
                {
                    try
                    {
                        str = serialPort.ReadLine();
                    } catch (TimeoutException)
                    {
                        try
                        {
                            str = serialPort.ReadExisting();
                        } catch
                        {
                            return;
                        }
                    } catch
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
            if (textBoxOutput.InvokeRequired)
            {
                var d = new writeTextSafeDelegate(writeTextSafe);
                textBoxOutput.Invoke(d, new object[] { text });
            }
            else
            {
                if (textBoxOutput.Text.Length == 0)
                {
                    textBoxOutput.AppendText(text);
                }
                else
                {
                    textBoxOutput.AppendText("\r\n" + text);
                }
                
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
            updatePortsStatusOnStatusBar();
            this.cache.connectionStartTimeFrom = DateTime.Now;
            toolStripStatusLabelTime.Text = "00:00:00";

            bHasWorkToDo = true;
            if (null == printThread)
            {
                printThread = new Thread(new ThreadStart(this.saveAndPrintDataThreadSafe));
                printThread.IsBackground = true;
                printThreadContinueRunning = true;
                printThread.Start();
            }

            timer1.Enabled = true;
            timer1.Start();
        }

        private void updatePortsStatusOnStatusBar()
        {
            if (this.statusStrip1.InvokeRequired)
            {
                var d = new updatePortsStatusOnStatusBarDelegate(updatePortsStatusOnStatusBar);
                this.statusStrip1.Invoke(d, new object[] { });
            } else
            {
                if (true == toolStripButtonConnect.Enabled)
                {
                    toolStripStatusLabelPort1.Text = "";
                    toolStripStatusLabelPort2.Text = "";
                    return;
                }
                ToolStripStatusLabel[] labels = { toolStripStatusLabelPort1, toolStripStatusLabelPort2 };
                System.IO.Ports.SerialPort[] ports = { this.serialPort1, this.serialPort2 };
                for (int i = 0; i < labels.Length; ++i)
                {
                    if (this.cache.selectedPortNames.Contains(ports[i].PortName))
                    {
                        if (ports[i].IsOpen)
                        {
                            labels[i].Text = ports[i].PortName;
                            labels[i].ForeColor = Color.Green;
                        }
                        else
                        {
                            labels[i].ForeColor = Color.Gray;
                        }
                    }
                    else
                    {
                        labels[i].Text = "";
                    }
                }
            }
        }

        // TODO: how to use SerialPort.DsrHolding
        // TODO: reconnect target ports when replug ports
        // TODO: show port's device name
        // TODO: add a button to clear log window
        // TODO: show target ports connection status

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
            updatePortsStatusOnStatusBar();
            bHasWorkToDo = false;
            this.timer1.Stop();
        }

        private void autoScrollDown(object sender, EventArgs e)
        {
            this.textBoxOutput.SelectionStart = textBoxOutput.TextLength;
            this.textBoxOutput.ScrollToCaret();
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
                bytes = BitConverter.GetBytes(longValue);
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

        private void serialPort2_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("serialPort2_ErrorReceived:" + e.ToString());
        }

        private void serialPort2_PinChanged(object sender, System.IO.Ports.SerialPinChangedEventArgs e)
        {
            Console.WriteLine("serialPort2_PinChanged:" + e.ToString());
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            this.textBoxOutput.Text = "";
            File.WriteAllText(this.cache.logFileName, string.Empty);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.toolStripButtonDisconnect.Enabled)
            {
                this.toolStripButtonDisconnect_Click(sender, e);
            }

            timer1.Stop();
            this.printThreadContinueRunning = false;
            this.bHasWorkToDo = false;
            if (null != this.printThread)
            {
                this.printThread.Join();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Size deltaSize = this.Size - this.mainFormInitializeSize;
            textBoxOutput.Size = textBoxOutputInitializeSize + deltaSize;
            textBoxInput.Location = new Point(textBoxInput.Location.X, this.textBoxInputInitializeLocation.Y + deltaSize.Height);
            textBoxInput.Size = new Size(this.textBoxInputInitializeSize.Width + deltaSize.Width, this.textBoxInputInitializeSize.Height);
            panel1.Location = panel1IntializedLocation + deltaSize;
            panel2.Location = new Point(panel2IntializedLocation.X + deltaSize.Width, panel2IntializedLocation.Y);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan timeSpan = DateTime.Now.Subtract(this.cache.connectionStartTimeFrom);
            int days = timeSpan.Days;
            if (0 == days)
            {
                this.toolStripStatusLabelTime.Text = timeSpan.ToString(@"hh\:mm\:ss");
            }
            else
            {
                this.toolStripStatusLabelTime.Text = timeSpan.ToString(@"dd\.hh\:mm\:ss");
            }
        }
    }
}
