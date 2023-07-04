
namespace USART_Monitor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPorts = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDisconnect = new System.Windows.Forms.ToolStripButton();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.comboBoxDataType = new System.Windows.Forms.ComboBox();
            this.comboBoxBaudrate = new System.Windows.Forms.ComboBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSendTo = new System.Windows.Forms.Label();
            this.comboBoxSendTo = new System.Windows.Forms.ComboBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSettings,
            this.toolStripButtonPorts,
            this.toolStripButtonConnect,
            this.toolStripButtonDisconnect});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "Ports";
            // 
            // toolStripButtonSettings
            // 
            this.toolStripButtonSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSettings.Image")));
            this.toolStripButtonSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSettings.Name = "toolStripButtonSettings";
            this.toolStripButtonSettings.Size = new System.Drawing.Size(53, 22);
            this.toolStripButtonSettings.Text = "Settings";
            this.toolStripButtonSettings.Click += new System.EventHandler(this.toolStripButtonSettings_Click);
            // 
            // toolStripButtonPorts
            // 
            this.toolStripButtonPorts.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonPorts.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPorts.Image")));
            this.toolStripButtonPorts.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPorts.Name = "toolStripButtonPorts";
            this.toolStripButtonPorts.Size = new System.Drawing.Size(38, 22);
            this.toolStripButtonPorts.Text = "Ports";
            this.toolStripButtonPorts.Click += new System.EventHandler(this.toolStripButtonPorts_Click);
            // 
            // toolStripButtonConnect
            // 
            this.toolStripButtonConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonConnect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonConnect.Image")));
            this.toolStripButtonConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConnect.Name = "toolStripButtonConnect";
            this.toolStripButtonConnect.Size = new System.Drawing.Size(56, 22);
            this.toolStripButtonConnect.Text = "Connect";
            this.toolStripButtonConnect.Click += new System.EventHandler(this.toolStripButtonConnect_Click);
            // 
            // toolStripButtonDisconnect
            // 
            this.toolStripButtonDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDisconnect.Image")));
            this.toolStripButtonDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDisconnect.Name = "toolStripButtonDisconnect";
            this.toolStripButtonDisconnect.Size = new System.Drawing.Size(70, 22);
            this.toolStripButtonDisconnect.Text = "Disconnect";
            this.toolStripButtonDisconnect.Click += new System.EventHandler(this.toolStripButtonDisconnect_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(this.serialPort1_errorReceived);
            this.serialPort1.PinChanged += new System.IO.Ports.SerialPinChangedEventHandler(this.serialPort1_pinChanged);
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_dataReceived);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox1.Location = new System.Drawing.Point(0, 25);
            this.textBox1.MaxLength = 0;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(784, 453);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.autoScrollDown);
            // 
            // serialPort2
            // 
            this.serialPort2.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort2_DataReceived);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(0, 487);
            this.textBoxInput.Multiline = true;
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInput.Size = new System.Drawing.Size(578, 46);
            this.textBoxInput.TabIndex = 3;
            // 
            // comboBoxDataType
            // 
            this.comboBoxDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDataType.FormattingEnabled = true;
            this.comboBoxDataType.Items.AddRange(new object[] {
            "text",
            "byte(8 bits)",
            "short(16 bits)",
            "int(32 bits)",
            "long(64 bits)",
            "float(32 bits)",
            "double(64 bits)"});
            this.comboBoxDataType.Location = new System.Drawing.Point(694, 513);
            this.comboBoxDataType.Name = "comboBoxDataType";
            this.comboBoxDataType.Size = new System.Drawing.Size(78, 21);
            this.comboBoxDataType.TabIndex = 4;
            // 
            // comboBoxBaudrate
            // 
            this.comboBoxBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaudrate.FormattingEnabled = true;
            this.comboBoxBaudrate.Items.AddRange(new object[] {
            "110",
            "134",
            "150",
            "200",
            "300",
            "600",
            "1200",
            "1800",
            "2400",
            "4800",
            "9600",
            "19200",
            "28800",
            "38400",
            "57600",
            "76800",
            "115200"});
            this.comboBoxBaudrate.Location = new System.Drawing.Point(597, 3);
            this.comboBoxBaudrate.Name = "comboBoxBaudrate";
            this.comboBoxBaudrate.Size = new System.Drawing.Size(78, 21);
            this.comboBoxBaudrate.TabIndex = 5;
            this.comboBoxBaudrate.SelectionChangeCommitted += new System.EventHandler(this.comboBoxBaudrate_SelectionChangeCommitted);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(584, 488);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(47, 46);
            this.buttonSend.TabIndex = 7;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(637, 517);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Data type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(540, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Baud rate";
            // 
            // labelSendTo
            // 
            this.labelSendTo.AutoSize = true;
            this.labelSendTo.Location = new System.Drawing.Point(640, 492);
            this.labelSendTo.Name = "labelSendTo";
            this.labelSendTo.Size = new System.Drawing.Size(44, 13);
            this.labelSendTo.TabIndex = 10;
            this.labelSendTo.Text = "Send to";
            // 
            // comboBoxSendTo
            // 
            this.comboBoxSendTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSendTo.FormattingEnabled = true;
            this.comboBoxSendTo.Location = new System.Drawing.Point(694, 488);
            this.comboBoxSendTo.Name = "comboBoxSendTo";
            this.comboBoxSendTo.Size = new System.Drawing.Size(78, 21);
            this.comboBoxSendTo.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.comboBoxSendTo);
            this.Controls.Add(this.labelSendTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.comboBoxBaudrate);
            this.Controls.Add(this.comboBoxDataType);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "USART Monitor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSettings;
        private System.Windows.Forms.ToolStripButton toolStripButtonPorts;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripButton toolStripButtonConnect;
        private System.Windows.Forms.ToolStripButton toolStripButtonDisconnect;
        private System.IO.Ports.SerialPort serialPort2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.ComboBox comboBoxDataType;
        private System.Windows.Forms.ComboBox comboBoxBaudrate;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelSendTo;
        private System.Windows.Forms.ComboBox comboBoxSendTo;
    }
}

