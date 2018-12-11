namespace SharpInjector
{
    partial class PrimaryForm
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
            this.ProcessLabel = new System.Windows.Forms.Label();
            this.ProcessSelectComboBox = new System.Windows.Forms.ComboBox();
            this.SelectDllButton = new System.Windows.Forms.Button();
            this.SelectedDllTextBox = new System.Windows.Forms.TextBox();
            this.InjectButton = new System.Windows.Forms.Button();
            this.PidCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ProcessLabel
            // 
            this.ProcessLabel.AutoSize = true;
            this.ProcessLabel.Location = new System.Drawing.Point(12, 41);
            this.ProcessLabel.Name = "ProcessLabel";
            this.ProcessLabel.Size = new System.Drawing.Size(48, 13);
            this.ProcessLabel.TabIndex = 0;
            this.ProcessLabel.Text = "Process:";
            // 
            // ProcessSelectComboBox
            // 
            this.ProcessSelectComboBox.FormattingEnabled = true;
            this.ProcessSelectComboBox.Location = new System.Drawing.Point(66, 38);
            this.ProcessSelectComboBox.Name = "ProcessSelectComboBox";
            this.ProcessSelectComboBox.Size = new System.Drawing.Size(206, 21);
            this.ProcessSelectComboBox.TabIndex = 1;
            this.ProcessSelectComboBox.DropDown += new System.EventHandler(this.ProcessSelectComboBox_DropDown);
            // 
            // SelectDllButton
            // 
            this.SelectDllButton.Location = new System.Drawing.Point(15, 74);
            this.SelectDllButton.Name = "SelectDllButton";
            this.SelectDllButton.Size = new System.Drawing.Size(75, 23);
            this.SelectDllButton.TabIndex = 2;
            this.SelectDllButton.Text = "Select DLL";
            this.SelectDllButton.UseVisualStyleBackColor = true;
            this.SelectDllButton.Click += new System.EventHandler(this.SelectDllButton_Click);
            // 
            // SelectedDllTextBox
            // 
            this.SelectedDllTextBox.Enabled = false;
            this.SelectedDllTextBox.Location = new System.Drawing.Point(96, 76);
            this.SelectedDllTextBox.Name = "SelectedDllTextBox";
            this.SelectedDllTextBox.Size = new System.Drawing.Size(313, 20);
            this.SelectedDllTextBox.TabIndex = 3;
            // 
            // InjectButton
            // 
            this.InjectButton.Location = new System.Drawing.Point(157, 135);
            this.InjectButton.Name = "InjectButton";
            this.InjectButton.Size = new System.Drawing.Size(115, 44);
            this.InjectButton.TabIndex = 4;
            this.InjectButton.Text = "Inject";
            this.InjectButton.UseVisualStyleBackColor = true;
            this.InjectButton.Click += new System.EventHandler(this.InjectButton_Click);
            // 
            // PidCheckBox
            // 
            this.PidCheckBox.AutoSize = true;
            this.PidCheckBox.Location = new System.Drawing.Point(278, 40);
            this.PidCheckBox.Name = "PidCheckBox";
            this.PidCheckBox.Size = new System.Drawing.Size(81, 17);
            this.PidCheckBox.TabIndex = 6;
            this.PidCheckBox.Text = "Display PID";
            this.PidCheckBox.UseVisualStyleBackColor = true;
            // 
            // PrimaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 212);
            this.Controls.Add(this.PidCheckBox);
            this.Controls.Add(this.InjectButton);
            this.Controls.Add(this.SelectedDllTextBox);
            this.Controls.Add(this.SelectDllButton);
            this.Controls.Add(this.ProcessSelectComboBox);
            this.Controls.Add(this.ProcessLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "PrimaryForm";
            this.Text = "Sharp Injector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ProcessLabel;
        private System.Windows.Forms.ComboBox ProcessSelectComboBox;
        private System.Windows.Forms.Button SelectDllButton;
        private System.Windows.Forms.TextBox SelectedDllTextBox;
        private System.Windows.Forms.Button InjectButton;
        private System.Windows.Forms.CheckBox PidCheckBox;
    }
}

