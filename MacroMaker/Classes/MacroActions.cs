using MacroMaker.Forms;
using MacroMaker.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroMaker.Classes
{
    public class MacroActions
    {
        WinHooks winHooks;
        RadialMenu radialMenu;
        NotificationManager notificationManager;
        public bool AutoRun { get; set; }

        internal MacroActions(WinHooks winHooks) 
        {
            this.winHooks = winHooks;
            notificationManager = new NotificationManager();
        }

        internal void PerformAction(InputButton inputButton)
        {
            if (inputButton.Action == "Copy")
            {
                winHooks.BlockMouseButtonIdsDown.Add(inputButton.DownId);
                winHooks.BlockMouseButtonIdsUp.Add(inputButton.UpId);
                Keyboard.PressKeyDown(0x11); // Ctrl key
                Keyboard.PressKeyDown((byte)Keys.C); // 'C' key
                Keyboard.PressKeyUp((byte)Keys.C);   // Release 'C' key
                Keyboard.PressKeyUp(0x11);   // Release Ctrl key
            }
            else if (inputButton.Action == "Paste")
            {
                winHooks.BlockMouseButtonIdsDown.Add(inputButton.DownId);
                winHooks.BlockMouseButtonIdsUp.Add(inputButton.UpId);
                Keyboard.PressKeyDown(0x11); // Ctrl key
                Keyboard.PressKeyDown((byte)Keys.V); // 'V' key
                Keyboard.PressKeyUp((byte)Keys.V); // Release 'V' key
                Keyboard.PressKeyUp(0x11);   // Release Ctrl key
            }
            else if (inputButton.Action == "None")
            {
                winHooks.BlockMouseButtonIdsDown.Add(inputButton.DownId);
                winHooks.BlockMouseButtonIdsUp.Add(inputButton.UpId);
            }
            else if (inputButton.Action == "Radial Menu")
            {
                winHooks.BlockMouseButtonIdsDown.Add(inputButton.DownId);
                winHooks.BlockMouseButtonIdsUp.Add(inputButton.UpId);
                Point mouseLocation = Cursor.Position; // Get the current mouse position

                using (RadialMenu radialMenu = new RadialMenu(mouseLocation, this))
                {
                    if (radialMenu != null && !radialMenu.IsDisposed)
                    {
                        // Disable auto run
                        if (AutoRun)
                        {
                            AutoRun = false;
                            notificationManager.ShowNotification("Auto Run Disabled");
                            Keyboard.PressKeyUp((byte)Keys.LShiftKey);
                        }
                        radialMenu.ShowDialog();
                    }
                }
            }
            else // Default
            {

            }
        }
    }
}
