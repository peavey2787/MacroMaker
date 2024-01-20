using MacroMaker.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroMaker.Forms
{
    public partial class EditRadialMenu : Form
    {
        private Panel lastClickedPanel;
        private Panel clickedPanel;
        private Control clickedControl;
        public EditRadialMenu()
        {
            InitializeComponent();

            // Attach the same event handler to all panels
            panel1.Paint += Panel_Paint;
            panel2.Paint += Panel_Paint;
            panel3.Paint += Panel_Paint;
            panel4.Paint += Panel_Paint;

            // Attach the Click event handler to all panels
            panel1.Click += Panel_Click;
            panel2.Click += Panel_Click;
            panel3.Click += Panel_Click;
            panel4.Click += Panel_Click;
        }


        #region Open/Close/Save
        private void SaveButton_Click(object sender, EventArgs e)
        {
            RadialOptions radialOptions = new RadialOptions();

            foreach (Panel panel in this.Controls.OfType<Panel>())
            {
                RadialOption radialOption = ExtractControlFromPanel(panel);
                radialOptions.Options.Add(radialOption);
            }

            AppSettings.Save<RadialOptions>("RadialOptions", radialOptions);
        }
        private RadialOption ExtractControlFromPanel(Panel panel)
        {
            RadialOption radialOption = new RadialOption();

            foreach (Control control in panel.Controls)
            {
                radialOption.Name = panel.Name;
                AControl aControl = new AControl();
                aControl.SetPropertiesFrom(control);
                radialOption.Controls.Add(aControl);
            }

            return radialOption;
        }
        private void RestoreControlsToPanel(List<Control> controls, Panel panel)
        {
            panel.Controls.Clear();

            foreach (Control control in controls)
            {
                control.Click += Control_Click;
                panel.Controls.Add(control);
            }
        }
        private void LoadRadialOptions()
        {
            RadialOptions radialOptions = AppSettings.Load<RadialOptions>("RadialOptions");

            if (radialOptions == null)
            {
                radialOptions = RadialOptions.GetDefaultRadialOptions();
            }

            foreach (RadialOption radialOption in radialOptions.Options)
            {
                Panel panel = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == radialOption.Name);

                if (panel != null)
                {
                    List<Control> controls = new List<Control>();
                    foreach (AControl aControl in radialOption.Controls)
                    {
                        var control = ControlFactory.CreateControl(aControl.Type);
                        control = aControl.CopyPropertiesTo(control);
                        controls.Add(control);
                    }

                    RestoreControlsToPanel(controls, panel);
                }
            }
        }
        private void EditRadialMenu_Load(object sender, EventArgs e)
        {
            LoadRadialOptions();
        }
        #endregion

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;

            if (panel == lastClickedPanel)
            {
                // Draw the border using the specified color
                using (Pen borderPen = new Pen(Color.Blue, 2))
                {
                    e.Graphics.DrawRectangle(borderPen, new Rectangle(0, 0, panel.Width, panel.Height));
                }
            }
        }
        private void Panel_Click(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            ClickPanel(panel);
        } 
        private void ClickPanel(Panel panel)
        {
            clickedPanel = panel;

            if (lastClickedPanel != null)
            {
                lastClickedPanel.Invalidate(); // Trigger repaint to remove the border
            }

            lastClickedPanel = panel;
            lastClickedPanel.Invalidate(); // Trigger repaint to show the border
        }


        #region actions
        // Actions
        private void OpenAppButton_Click(object sender, EventArgs e)
        {
            if (clickedPanel == null) return;

            var control = ControlFactory.CreateControl("Button");
            clickedControl = control;
            control.Name = RadialAction.Actions.OpenApp.ToString();
            control.Text = "Click Me";
            control.Location = new Point(100 - (control.Width / 2), 25);
            control.Click += Control_Click;

            clickedPanel.Controls.Add(control);
            clickedPanel.Invalidate();
        }
        private void AudioPlaybackButton_Click(object sender, EventArgs e)
        {
            if (clickedPanel == null) return;

            var control = ControlFactory.CreateControl("ComboBox");
            clickedControl = control;
            control.Name = RadialAction.Actions.ChangeDefaultAudioPlayback.ToString();
            control.Text = "Audio Devices";
            control.Location = new Point(100 - (control.Width / 2), 25);
            control.Click += Control_Click;

            clickedPanel.Controls.Add(control);
            clickedPanel.Invalidate();
        }
        private void AudioMicButton_Click(object sender, EventArgs e)
        {
            if (clickedPanel == null) return;

            var control = ControlFactory.CreateControl("ComboBox");
            clickedControl = control;
            control.Name = RadialAction.Actions.ChangeDefaultAudioMic.ToString();
            control.Text = "Mic Devices";
            control.Location = new Point(100 - (control.Width / 2), 25);
            control.Click += Control_Click;

            clickedPanel.Controls.Add(control);
            clickedPanel.Invalidate();
        }
        #endregion


        // Labels/Extra
        private void AddTextButton_Click(object sender, EventArgs e)
        {
            if (clickedPanel == null) return;

            var control = ControlFactory.CreateControl("Label");
            clickedControl = control;
            control.Name = RadialAction.Actions.None.ToString();
            control.Text = "Text";
            control.Location = new Point(100 - (control.Width / 2), 25);
            control.Click += Control_Click;

            clickedPanel.Controls.Add(control);
            clickedPanel.Invalidate();
        }

        private void Control_Click(object sender, EventArgs e)
        {
            clickedControl = sender as Control;
            ClickPanel(clickedControl.Parent as Panel);
            ControlNameLabel.Text = clickedControl.Name;
            if (clickedControl != null)
            {
                // Determine the type of the clicked control
                switch (clickedControl.GetType().Name)
                {
                    case "Label":
                        TextTextBox.Text = (clickedControl as Label)?.Text;
                        break;
                    case "TextBox":
                        TextTextBox.Text = (clickedControl as TextBox)?.Text;
                        if (clickedControl.Name == RadialAction.Actions.OpenApp.ToString())
                        {
                            FilePathTextBox.Text = clickedControl.Tag.ToString();
                        }
                        break;
                }
            }
        }




        #region Customize Selected Control
        // Customize Control
        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            if(clickedControl == null) return;

            clickedControl.Location = new Point(clickedControl.Location.X, clickedControl.Location.Y + 5);
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            if (clickedControl == null) return;

            clickedControl.Location = new Point(clickedControl.Location.X, clickedControl.Location.Y - 5);
        }

        private void MoveLeftButton_Click(object sender, EventArgs e)
        {
            if (clickedControl == null) return;

            clickedControl.Location = new Point(clickedControl.Location.X - 5, clickedControl.Location.Y);
        }

        private void MoveRightButton_Click(object sender, EventArgs e)
        {
            if (clickedControl == null) return;

            clickedControl.Location = new Point(clickedControl.Location.X + 5, clickedControl.Location.Y);
        }

        private void IncreaseHeightButton_Click(object sender, EventArgs e)
        {
            if (clickedControl == null) return;

            clickedControl.Height += 5;
        }
        private void DecreaseHeightButton_Click(object sender, EventArgs e)
        {
            if (clickedControl == null) return;

            clickedControl.Height -= 5;
        }
        private void DecreaseWidthButton_Click(object sender, EventArgs e)
        {
            if (clickedControl == null) return;

            clickedControl.Width -= 5;
        }
        private void IncreaseWidthButton_Click(object sender, EventArgs e)
        {
            if (clickedControl == null) return;

            clickedControl.Width += 5;
        }
        private void FilePathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (clickedControl == null) return;

            clickedControl.Tag = FilePathTextBox.Text;
        }
        private void TextTextBox_TextChanged(object sender, EventArgs e)
        {
            if (clickedControl == null) return;

            clickedControl.Text = TextTextBox.Text;
        }
        private void FontColorButton_Click(object sender, EventArgs e)
        {
            if (clickedControl == null) return;

            clickedControl.ForeColor = ShowColorPicker(clickedControl.ForeColor);
        }
        private void BackgroundColorButton_Click(object sender, EventArgs e)
        {
            if (clickedControl == null) return;

            clickedControl.BackColor = ShowColorPicker(clickedControl.BackColor);            
        }
        private void IncreaseFontButton_Click(object sender, EventArgs e)
        {
            if (clickedControl == null) return;
            clickedControl.Font = new Font(clickedControl.Font.FontFamily, clickedControl.Font.Size + 2, FontStyle.Regular);
        }

        private void DecreaseFontButton_Click(object sender, EventArgs e)
        {
            if (clickedControl == null) return;
            clickedControl.Font = new Font(clickedControl.Font.FontFamily, clickedControl.Font.Size - 2, FontStyle.Regular);
        }
        #endregion




        // Helpers
        private Color ShowColorPicker(Color startingColor)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                // Get the standard colors
                int[] standardColors = colorDialog.CustomColors.Cast<int>().ToArray();

                // Add your custom colors
                Color[] customColors = new Color[] { Color.Beige };
                int[] customColorsOle = customColors.Select(c => ColorTranslator.ToOle(c)).ToArray();

                // Combine custom colors and standard colors
                colorDialog.CustomColors = customColorsOle.Concat(standardColors).ToArray();


                colorDialog.Color = startingColor;

                DialogResult result = colorDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    Color selectedColor = colorDialog.Color;

                    return selectedColor;
                }
            }
            return startingColor;
        }




    }
}
