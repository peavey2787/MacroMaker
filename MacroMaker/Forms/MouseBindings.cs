using MacroMaker.Classes;
using MacroMaker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroMaker.Forms
{
    public partial class MouseBindings : Form
    {
        #region Variables
        WinHooks winHooks;
        Mouse mouse;
        InputButton buttonToSave;
        int mouseMoveId = 0;
        bool buttonDown = true;
        bool toggle = false;
        bool appControl = false;
        private List<int> buttonHistory = new List<int>();
        private const int HistorySize = 4;

        internal Mouse GetMouse() { return mouse; }
        internal void SetMouse(Mouse mouse) { this.mouse = mouse; }
        #endregion


        #region Start/Load/Close
        public MouseBindings(WinHooks winHooks)
        {
            InitializeComponent();
            this.winHooks = winHooks;
        }
        private void MouseBindings_Load(object sender, EventArgs e)
        {
            appControl = true; // Prevent triggering index/check changed events
            if(mouse.Buttons == null) mouse.LoadDefaults();
            foreach (InputButton button in mouse.Buttons)
            {
                CreateButtonControls(button);
            }           
            
            appControl = false;
        }
        private void MouseBindings_FormClosing(object sender, FormClosingEventArgs e)
        {
            mouse.SaveSettings();
        }
        #endregion


        #region Bindings Panel
        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            buttonToSave.DownId = buttonHistory[0];
            buttonToSave.UpId = buttonHistory[1];

            CloseBindingsPanel();
        }
        private void OpenBindingsPanel(InputButton buttonToSave)
        {
            InputButton mouseMoveButton = mouse.Buttons.Find(b => b.Name.Equals("Mouse Move"));
            mouseMoveId = mouseMoveButton.DownId;

            ButtonNameLabel.Text = buttonToSave.Name;
            BindPanel.Visible = true;
            winHooks.MouseButtonDown += HandleMouseButtonDown;
            ButtonPressedLabel.Text = buttonToSave.UpId.ToString();
            toggle = !toggle;
        }
        private void CloseBindingsPanel()
        {
            BindPanel.Visible = false;
            winHooks.MouseButtonDown -= HandleMouseButtonDown;
            toggle = !toggle;
            buttonDown = true;
        }
        private void ToggleBindingsPanel(InputButton buttonToSave)
        {
            if (!toggle)            
                OpenBindingsPanel(buttonToSave);            
            else
                CloseBindingsPanel();
        }
        private void AssignButtonIds(int button)
        {
            if (buttonToSave.Name == "Mouse Move")
            {
                buttonToSave.DownId = button;
                buttonToSave.UpId = button;
                buttonDown = false;
                return;
            }
            
            if (buttonDown)
            {
                buttonToSave.DownId = button;         
            }
            else
            {
                buttonToSave.UpId = button;
            }
        }
        private void HandleMouseButtonDown(int button)
        {
            // Ignore the mouse move button id if not assigning it
            if (buttonToSave.Name != "Mouse Move" && button == mouseMoveId)
                return;
                       
            ButtonPressedLabel.Text = button.ToString();
                        
            AssignButtonIds(button);
            
            buttonDown = !buttonDown;

            // Add the current button ID to the history
            buttonHistory.Add(button);

            // Trim the history to keep only the last 4 button IDs
            if (buttonHistory.Count > HistorySize)
            {
                buttonHistory.RemoveAt(0);
            }
        }
        #endregion

        #region Button Controls
        // Button controls interaction
        private void ButtonLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (appControl) return;

            var linkLabel = sender as LinkLabel;
            string buttonName = linkLabel.Text;
            buttonToSave = mouse.Buttons.Find(b => b.Name.Equals(buttonName));

            if (buttonToSave == null)
            {
                buttonToSave = new InputButton(buttonName);
                mouse.Buttons.Add(buttonToSave);
            }

            ToggleBindingsPanel(buttonToSave);
        }
        private void ButtonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (appControl) return;

            var comboBox = sender as ComboBox;
            string buttonName = comboBox.Name.Remove(comboBox.Name.IndexOf("ComboBox"));
            buttonToSave = mouse.Buttons.Find(b => b.Name.Equals(buttonName));

            if (buttonToSave == null)
            {
                buttonToSave = new InputButton(buttonName);
                mouse.Buttons.Add(buttonToSave);
            }

            buttonToSave.Action = comboBox.Text;
        }
        private void ButtonCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (appControl) return;

            var checkBox = sender as CheckBox;
            string buttonName = checkBox.Name.Remove(checkBox.Name.IndexOf("CheckBox"));
            buttonToSave = mouse.Buttons.Find(b => b.Name.Equals(buttonName));

            if (buttonToSave == null)
            {
                buttonToSave = new InputButton(buttonName);
                mouse.Buttons.Add(buttonToSave);
            }

            buttonToSave.PreventDoubleClick = checkBox.Checked;
        }


        // Button controls creation
        int startX = 50;
        int startY = 20;
        int margin = 0;
        int rows = 3;
        int rowCount = 0;
        int rowMargin = 150;
        private void CreateButtonControls(InputButton inputButton)
        {
            margin = ButtonComboBox.Height + 10;

            // Create copies of the original controls
            ComboBox newComboBox = new ComboBox();
            LinkLabel newLinkLabel = new LinkLabel();
            CheckBox newCheckBox = new CheckBox();

            // Copy properties from the original controls to the new controls
            CopyControlProperties(ButtonComboBox, newComboBox);
            CopyControlProperties(ButtonLinkLabel, newLinkLabel);
            CopyControlProperties(ButtonCheckBox, newCheckBox);

            newComboBox.Visible = true;
            newLinkLabel.Visible = true;
            newCheckBox.Visible = true;

            // Set name
            newComboBox.Name = inputButton.Name + "ComboBox";
            newLinkLabel.Name = inputButton.Name + "LinkLabel";
            newCheckBox.Name = inputButton.Name + "CheckBox";

            // Set text
            newLinkLabel.Text = inputButton.Name;

            // Set selected action/macro          
            newComboBox.SelectedItem = inputButton.Action;

            // Set checkbox            
            newCheckBox.Checked = inputButton.PreventDoubleClick;

            // Set the positions
            newLinkLabel.Location = new Point(startX, startY);
            startY += margin;
            newComboBox.Location = new Point(startX, startY);
            startY += margin;
            newCheckBox.Location = new Point(startX, startY);
            startY += margin * 2;

            // Add the copied controls to the form
            ButtonPanel.Controls.Add(newLinkLabel);
            if (inputButton.Name != "Mouse Move")
            {
                ButtonPanel.Controls.Add(newComboBox);
                ButtonPanel.Controls.Add(newCheckBox);
            }

            if(rowCount == rows)
            {
                rowCount = 0;
                startX += rowMargin;
                startY = 20;
            }    
            rowCount++;
        }
        private void CopyControlProperties(Control sourceControl, Control destinationControl)
        {
            // Copy common properties
            destinationControl.Bounds = sourceControl.Bounds;
            destinationControl.Enabled = sourceControl.Enabled;
            destinationControl.Visible = sourceControl.Visible;

            // Special handling for copying items from ComboBox
            if (sourceControl is ComboBox sourceComboBox && destinationControl is ComboBox destinationComboBox)
            {
                destinationComboBox.Items.Clear();
                foreach (var item in sourceComboBox.Items)
                {
                    destinationComboBox.Items.Add(item);
                }

                // Copy the Click event handler
                destinationComboBox.SelectedIndexChanged += ButtonComboBox_SelectedIndexChanged;
            }

            // Special handling for copying Checked state from CheckBox
            if (sourceControl is CheckBox sourceCheckBox && destinationControl is CheckBox destinationCheckBox)
            {
                destinationCheckBox.Checked = sourceCheckBox.Checked;
                destinationCheckBox.Text = sourceCheckBox.Text;

                // Copy the Click event handler
                destinationCheckBox.Click += ButtonCheckBox_CheckedChanged;
            }

            // Special handling for copying Text property from LinkLabel
            if (sourceControl is LinkLabel sourceLinkLabel && destinationControl is LinkLabel destinationLinkLabel)
            {
                destinationLinkLabel.Text = sourceLinkLabel.Text;

                destinationLinkLabel.LinkClicked += ButtonLinkLabel_LinkClicked;
            }
        }
        #endregion

    }
}
