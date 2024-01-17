using MacroMaker.Classes;
using MacroMaker.Forms;
using MacroMaker.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroMaker
{
    public partial class Form1 : Form
    {
        private WinHooks winHooks;
        Mouse mouse;
        MacroActions macroActions;
        public Form1()
        {
            InitializeComponent();

            mouse = new Mouse();
            mouse.LoadFromSettings();

            winHooks = new WinHooks();
            winHooks.MouseButtonDown += HandleMouseButtonDown;
            winHooks.KeyDown += HandleKeyDown;


            macroActions = new MacroActions(winHooks);
        }

        private void HandleMouseButtonDown(int button)
        {
            if (mouse.Buttons == null) return;

            InputButton inputButton = mouse.Buttons.Find(b => (b.DownId == button) || (b.UpId == button));
            if (inputButton != null)
            {
                inputButton.IsDown = !inputButton.IsDown;
            }
            else
                return;

            // Do nothing on prevent dbl clicks
            if (inputButton.IsDown && inputButton.PreventDoubleClick)
            {
                return;
            }

            ActiveMouseButtonLabel.Text = inputButton.Name;
            ActiveMouseMacroLabel.Text = inputButton.Action;

            if (inputButton.Name == "Mouse Move")
            {
                ActiveMouseButtonLabel.Text += $" {Cursor.Position.X},{Cursor.Position.Y}";
                return;
            }

            macroActions.PerformAction(inputButton);          
        }
        private void HandleKeyDown(int keyCode)
        {
            // Convert the byte value back to Keys enumeration
            Keys keyPressed = (Keys)Enum.ToObject(typeof(Keys), (byte)keyCode);
            ActiveKeyboardButtonLabel.Text = keyPressed.ToString();
        }


        private void MouseBindingsButton_Click(object sender, EventArgs e)
        {
            MouseBindings mouseBindings = new MouseBindings(winHooks);
            mouseBindings.FormClosing += MouseBindings_FormClosing;
            mouseBindings.SetMouse(mouse);
            mouseBindings.ShowDialog();            
        }

        private void MouseBindings_FormClosing(object sender, FormClosingEventArgs e)
        {
            var form = sender as MouseBindings;
            mouse = form.GetMouse();            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            winHooks.Dispose();
        }
    }
}
