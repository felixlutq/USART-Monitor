
namespace USART_Monitor
{
    partial class SettingsForm
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.textBoxLogFileName = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.LogFile = new System.Windows.Forms.Label();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.checkBoxDateTime = new System.Windows.Forms.CheckBox();
            this.checkBoxShowRXTX = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBoxLogFileName
            // 
            this.textBoxLogFileName.Location = new System.Drawing.Point(68, 77);
            this.textBoxLogFileName.Name = "textBoxLogFileName";
            this.textBoxLogFileName.Size = new System.Drawing.Size(100, 20);
            this.textBoxLogFileName.TabIndex = 4;
            this.textBoxLogFileName.Text = "log.txt";
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(174, 77);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(60, 23);
            this.Browse.TabIndex = 5;
            this.Browse.Text = "Browse...";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // LogFile
            // 
            this.LogFile.AutoSize = true;
            this.LogFile.Location = new System.Drawing.Point(18, 80);
            this.LogFile.Name = "LogFile";
            this.LogFile.Size = new System.Drawing.Size(44, 13);
            this.LogFile.TabIndex = 6;
            this.LogFile.Text = "Log File";
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(37, 123);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 7;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(141, 123);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Cancel";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // checkBoxDateTime
            // 
            this.checkBoxDateTime.AutoSize = true;
            this.checkBoxDateTime.Location = new System.Drawing.Point(46, 45);
            this.checkBoxDateTime.Name = "checkBoxDateTime";
            this.checkBoxDateTime.Size = new System.Drawing.Size(134, 17);
            this.checkBoxDateTime.TabIndex = 9;
            this.checkBoxDateTime.Text = "show time for each line";
            this.checkBoxDateTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowRXTX
            // 
            this.checkBoxShowRXTX.AutoSize = true;
            this.checkBoxShowRXTX.Location = new System.Drawing.Point(46, 22);
            this.checkBoxShowRXTX.Name = "checkBoxShowRXTX";
            this.checkBoxShowRXTX.Size = new System.Drawing.Size(107, 17);
            this.checkBoxShowRXTX.TabIndex = 10;
            this.checkBoxShowRXTX.Text = "show RX and TX";
            this.checkBoxShowRXTX.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 163);
            this.Controls.Add(this.checkBoxShowRXTX);
            this.Controls.Add(this.checkBoxDateTime);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.LogFile);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.textBoxLogFileName);
            this.Name = "SettingsForm";
            this.Text = "Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;
        private System.Windows.Forms.TextBox textBoxLogFileName;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Label LogFile;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.CheckBox checkBoxDateTime;
        private System.Windows.Forms.CheckBox checkBoxShowRXTX;
    }
}