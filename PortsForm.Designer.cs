
namespace USART_Monitor
{
    partial class PortsForm
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
            this.checkedListBoxPortsList = new System.Windows.Forms.CheckedListBox();
            this.applyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListBoxPortsList
            // 
            this.checkedListBoxPortsList.CheckOnClick = true;
            this.checkedListBoxPortsList.FormattingEnabled = true;
            this.checkedListBoxPortsList.Location = new System.Drawing.Point(12, 12);
            this.checkedListBoxPortsList.Name = "checkedListBoxPortsList";
            this.checkedListBoxPortsList.Size = new System.Drawing.Size(187, 259);
            this.checkedListBoxPortsList.Sorted = true;
            this.checkedListBoxPortsList.TabIndex = 0;
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(58, 277);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 1;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // PortsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 309);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.checkedListBoxPortsList);
            this.Name = "PortsForm";
            this.Text = "PortsForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxPortsList;
        private System.Windows.Forms.Button applyButton;
    }
}