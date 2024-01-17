namespace MacroMaker
{
    partial class Form1
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
            this.MouseBindingsButton = new System.Windows.Forms.Button();
            this.ActiveMouseLabel = new System.Windows.Forms.Label();
            this.ActiveKeyboardLabel = new System.Windows.Forms.Label();
            this.KeyboardBindingsButton = new System.Windows.Forms.Button();
            this.ActiveMouseButtonLabel = new System.Windows.Forms.Label();
            this.ActiveKeyboardButtonLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ActiveMouseMacroLabel = new System.Windows.Forms.Label();
            this.ActiveKeyboardMacroLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MouseBindingsButton
            // 
            this.MouseBindingsButton.Location = new System.Drawing.Point(67, 36);
            this.MouseBindingsButton.Name = "MouseBindingsButton";
            this.MouseBindingsButton.Size = new System.Drawing.Size(78, 39);
            this.MouseBindingsButton.TabIndex = 1;
            this.MouseBindingsButton.Text = "Mouse Bindings";
            this.MouseBindingsButton.UseVisualStyleBackColor = true;
            this.MouseBindingsButton.Click += new System.EventHandler(this.MouseBindingsButton_Click);
            // 
            // ActiveMouseLabel
            // 
            this.ActiveMouseLabel.AutoSize = true;
            this.ActiveMouseLabel.Location = new System.Drawing.Point(58, 119);
            this.ActiveMouseLabel.Name = "ActiveMouseLabel";
            this.ActiveMouseLabel.Size = new System.Drawing.Size(73, 13);
            this.ActiveMouseLabel.TabIndex = 2;
            this.ActiveMouseLabel.Text = "Mouse Button";
            // 
            // ActiveKeyboardLabel
            // 
            this.ActiveKeyboardLabel.AutoSize = true;
            this.ActiveKeyboardLabel.Location = new System.Drawing.Point(329, 119);
            this.ActiveKeyboardLabel.Name = "ActiveKeyboardLabel";
            this.ActiveKeyboardLabel.Size = new System.Drawing.Size(86, 13);
            this.ActiveKeyboardLabel.TabIndex = 4;
            this.ActiveKeyboardLabel.Text = "Keyboard Button";
            // 
            // KeyboardBindingsButton
            // 
            this.KeyboardBindingsButton.Location = new System.Drawing.Point(358, 36);
            this.KeyboardBindingsButton.Name = "KeyboardBindingsButton";
            this.KeyboardBindingsButton.Size = new System.Drawing.Size(78, 39);
            this.KeyboardBindingsButton.TabIndex = 5;
            this.KeyboardBindingsButton.Text = "Keyboard Bindings";
            this.KeyboardBindingsButton.UseVisualStyleBackColor = true;
            // 
            // ActiveMouseButtonLabel
            // 
            this.ActiveMouseButtonLabel.AutoSize = true;
            this.ActiveMouseButtonLabel.Location = new System.Drawing.Point(137, 119);
            this.ActiveMouseButtonLabel.Name = "ActiveMouseButtonLabel";
            this.ActiveMouseButtonLabel.Size = new System.Drawing.Size(33, 13);
            this.ActiveMouseButtonLabel.TabIndex = 6;
            this.ActiveMouseButtonLabel.Text = "None";
            // 
            // ActiveKeyboardButtonLabel
            // 
            this.ActiveKeyboardButtonLabel.AutoSize = true;
            this.ActiveKeyboardButtonLabel.Location = new System.Drawing.Point(421, 119);
            this.ActiveKeyboardButtonLabel.Name = "ActiveKeyboardButtonLabel";
            this.ActiveKeyboardButtonLabel.Size = new System.Drawing.Size(33, 13);
            this.ActiveKeyboardButtonLabel.TabIndex = 7;
            this.ActiveKeyboardButtonLabel.Text = "None";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Macro";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(329, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Macro";
            // 
            // ActiveMouseMacroLabel
            // 
            this.ActiveMouseMacroLabel.AutoSize = true;
            this.ActiveMouseMacroLabel.Location = new System.Drawing.Point(137, 146);
            this.ActiveMouseMacroLabel.Name = "ActiveMouseMacroLabel";
            this.ActiveMouseMacroLabel.Size = new System.Drawing.Size(33, 13);
            this.ActiveMouseMacroLabel.TabIndex = 10;
            this.ActiveMouseMacroLabel.Text = "None";
            // 
            // ActiveKeyboardMacroLabel
            // 
            this.ActiveKeyboardMacroLabel.AutoSize = true;
            this.ActiveKeyboardMacroLabel.Location = new System.Drawing.Point(421, 146);
            this.ActiveKeyboardMacroLabel.Name = "ActiveKeyboardMacroLabel";
            this.ActiveKeyboardMacroLabel.Size = new System.Drawing.Size(33, 13);
            this.ActiveKeyboardMacroLabel.TabIndex = 11;
            this.ActiveKeyboardMacroLabel.Text = "None";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ActiveKeyboardMacroLabel);
            this.Controls.Add(this.ActiveMouseMacroLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ActiveKeyboardButtonLabel);
            this.Controls.Add(this.ActiveMouseButtonLabel);
            this.Controls.Add(this.KeyboardBindingsButton);
            this.Controls.Add(this.ActiveKeyboardLabel);
            this.Controls.Add(this.ActiveMouseLabel);
            this.Controls.Add(this.MouseBindingsButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button MouseBindingsButton;
        private System.Windows.Forms.Label ActiveMouseLabel;
        private System.Windows.Forms.Label ActiveKeyboardLabel;
        private System.Windows.Forms.Button KeyboardBindingsButton;
        private System.Windows.Forms.Label ActiveMouseButtonLabel;
        private System.Windows.Forms.Label ActiveKeyboardButtonLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ActiveMouseMacroLabel;
        private System.Windows.Forms.Label ActiveKeyboardMacroLabel;
    }
}

