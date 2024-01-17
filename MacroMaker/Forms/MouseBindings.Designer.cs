namespace MacroMaker.Forms
{
    partial class MouseBindings
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
            this.ButtonLinkLabel = new System.Windows.Forms.LinkLabel();
            this.BindPanel = new System.Windows.Forms.Panel();
            this.ButtonNameLabel = new System.Windows.Forms.Label();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonPressedLabel = new System.Windows.Forms.Label();
            this.ButtonComboBox = new System.Windows.Forms.ComboBox();
            this.ButtonCheckBox = new System.Windows.Forms.CheckBox();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.BindPanel.SuspendLayout();
            this.ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonLinkLabel
            // 
            this.ButtonLinkLabel.AutoSize = true;
            this.ButtonLinkLabel.Location = new System.Drawing.Point(23, 17);
            this.ButtonLinkLabel.Name = "ButtonLinkLabel";
            this.ButtonLinkLabel.Size = new System.Drawing.Size(69, 13);
            this.ButtonLinkLabel.TabIndex = 0;
            this.ButtonLinkLabel.TabStop = true;
            this.ButtonLinkLabel.Text = "Button Name";
            this.ButtonLinkLabel.Visible = false;
            this.ButtonLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ButtonLinkLabel_LinkClicked);
            // 
            // BindPanel
            // 
            this.BindPanel.Controls.Add(this.ButtonNameLabel);
            this.BindPanel.Controls.Add(this.ConfirmButton);
            this.BindPanel.Controls.Add(this.label1);
            this.BindPanel.Controls.Add(this.ButtonPressedLabel);
            this.BindPanel.Location = new System.Drawing.Point(262, 318);
            this.BindPanel.Name = "BindPanel";
            this.BindPanel.Size = new System.Drawing.Size(251, 120);
            this.BindPanel.TabIndex = 1;
            this.BindPanel.Visible = false;
            // 
            // ButtonNameLabel
            // 
            this.ButtonNameLabel.AutoSize = true;
            this.ButtonNameLabel.Location = new System.Drawing.Point(90, 7);
            this.ButtonNameLabel.Name = "ButtonNameLabel";
            this.ButtonNameLabel.Size = new System.Drawing.Size(69, 13);
            this.ButtonNameLabel.TabIndex = 4;
            this.ButtonNameLabel.Text = "Button Name";
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Location = new System.Drawing.Point(91, 92);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(60, 24);
            this.ConfirmButton.TabIndex = 2;
            this.ConfirmButton.Text = "Confirm";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Button Id";
            // 
            // ButtonPressedLabel
            // 
            this.ButtonPressedLabel.AutoSize = true;
            this.ButtonPressedLabel.Location = new System.Drawing.Point(97, 61);
            this.ButtonPressedLabel.Name = "ButtonPressedLabel";
            this.ButtonPressedLabel.Size = new System.Drawing.Size(49, 13);
            this.ButtonPressedLabel.TabIndex = 0;
            this.ButtonPressedLabel.Text = "8832492";
            // 
            // ButtonComboBox
            // 
            this.ButtonComboBox.FormattingEnabled = true;
            this.ButtonComboBox.Items.AddRange(new object[] {
            "Copy",
            "Paste",
            "Default",
            "Radial Menu",
            "None"});
            this.ButtonComboBox.Location = new System.Drawing.Point(12, 33);
            this.ButtonComboBox.Name = "ButtonComboBox";
            this.ButtonComboBox.Size = new System.Drawing.Size(85, 21);
            this.ButtonComboBox.TabIndex = 5;
            this.ButtonComboBox.Text = "Default";
            this.ButtonComboBox.Visible = false;
            this.ButtonComboBox.SelectedIndexChanged += new System.EventHandler(this.ButtonComboBox_SelectedIndexChanged);
            // 
            // ButtonCheckBox
            // 
            this.ButtonCheckBox.AutoSize = true;
            this.ButtonCheckBox.Location = new System.Drawing.Point(12, 60);
            this.ButtonCheckBox.Name = "ButtonCheckBox";
            this.ButtonCheckBox.Size = new System.Drawing.Size(91, 17);
            this.ButtonCheckBox.TabIndex = 11;
            this.ButtonCheckBox.Text = "Double click?";
            this.ButtonCheckBox.UseVisualStyleBackColor = true;
            this.ButtonCheckBox.Visible = false;
            this.ButtonCheckBox.CheckedChanged += new System.EventHandler(this.ButtonCheckBox_CheckedChanged);
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.AutoScroll = true;
            this.ButtonPanel.Controls.Add(this.ButtonComboBox);
            this.ButtonPanel.Controls.Add(this.ButtonCheckBox);
            this.ButtonPanel.Controls.Add(this.ButtonLinkLabel);
            this.ButtonPanel.Location = new System.Drawing.Point(0, 0);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(800, 312);
            this.ButtonPanel.TabIndex = 12;
            // 
            // MouseBindings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ButtonPanel);
            this.Controls.Add(this.BindPanel);
            this.Name = "MouseBindings";
            this.Text = "MouseBindings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MouseBindings_FormClosing);
            this.Load += new System.EventHandler(this.MouseBindings_Load);
            this.BindPanel.ResumeLayout(false);
            this.BindPanel.PerformLayout();
            this.ButtonPanel.ResumeLayout(false);
            this.ButtonPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel ButtonLinkLabel;
        private System.Windows.Forms.Panel BindPanel;
        private System.Windows.Forms.Label ButtonPressedLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.ComboBox ButtonComboBox;
        private System.Windows.Forms.CheckBox ButtonCheckBox;
        private System.Windows.Forms.Label ButtonNameLabel;
        private System.Windows.Forms.Panel ButtonPanel;
    }
}