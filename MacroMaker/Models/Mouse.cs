namespace MacroMaker.Models
{
    using MacroMaker.Classes;
    using MacroMaker.Forms;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    internal interface IInputDevice
    {
        List<InputButton> Buttons { get; set; }
    }

    internal class InputButton
    {
        public string Name { get; set; }
        public int DownId { get; set; }
        public int UpId { get; set; }
        public bool IsDown { get; set; } = false;
        public bool PreventDoubleClick { get; set; }
        public string Action { get; set; } = "Default";

        public InputButton() { }
        public InputButton(string name)
        {
            Name = name;
        }
    }

    internal class Mouse : IInputDevice
    {
        public List<InputButton> Buttons { get; set; }
        public Mouse()
        {

        }        
        internal void LoadDefaults()
        {
            Buttons = new List<InputButton>();

            var inputButton = new InputButton("Mouse Move");
            inputButton.DownId = 512;
            inputButton.UpId = 512;
            Buttons.Add(inputButton);

            // Default buttons
            Buttons.Add(new InputButton("Left Click"));
            Buttons.Add(new InputButton("Right Click"));
            Buttons.Add(new InputButton("Wheel Up"));
            Buttons.Add(new InputButton("Wheel Down"));
            Buttons.Add(new InputButton("Wheel Click"));
            Buttons.Add(new InputButton("Side Button 1 Click"));
            Buttons.Add(new InputButton("Side Button 2 Click"));
        }
        internal void SetMouse(Mouse mouse)
        {
            Buttons = new List<InputButton>();
            Buttons = mouse.Buttons;
        }
        internal bool LoadFromSettings()
        {
            Mouse mouse = AppSettings.Load<Mouse>("MouseBindings");

            if (mouse == null) mouse = new Mouse();

            SetMouse(mouse);

            return true;
        }
        internal void SaveSettings()
        {
            // Save bindings
            AppSettings.Save<Mouse>("MouseBindings", this);
        }
    }
}