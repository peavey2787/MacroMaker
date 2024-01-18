using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MacroMaker.Classes;
using NAudio.CoreAudioApi;
using static MacroMaker.Forms.RadialAction;

namespace MacroMaker.Forms
{
    public partial class RadialMenu : Form
    {
        MacroActions macroActions;
        NotificationManager notificationManager;
        AudioDeviceManager audioDeviceManager;
        private static bool isMenuOpen = false;
        private const int DeadZoneRadius = 20;
        private const int MenuRadius = 20;
        private int activeSlice = -1;
        Point centerLocation;
        private Point formCenter
        {
            get
            {
                return new Point(this.Width / 2, this.Height / 2);
            }
        }
        public bool IsMenuOpen()
        {
            return isMenuOpen;
        }
        public RadialMenu(Point mouseLocation, MacroActions macroActions)
        {            
            InitializeComponent();
            this.macroActions = macroActions;
            notificationManager = new NotificationManager();

            // Check if the menu is already open, and if so, close the new instance
            if (isMenuOpen)
            {
                Close();
                return;
            }

            // If not, mark the menu as open and proceed with initialization
            isMenuOpen = true;

            centerLocation = mouseLocation;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;

            InitializeRadialMenu();
        }

        private void InitializeRadialMenu()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;

            Size = new Size(400, 400);

            // Calculate the location to center the form based on centerLocation
            int x = (centerLocation.X - Width / 2) - DeadZoneRadius;
            int y = (centerLocation.Y - Height / 2) - DeadZoneRadius;

            Location = new Point(x, y);

            // Set the background color to the desired color
            BackColor = Color.Beige;

            // Make the background color transparent
            TransparencyKey = Color.Beige;

            MouseMove += RadialButtonMenu_MouseMove;
            MouseDown += RadialButtonMenu_MouseDown;
            Paint += RadialButtonMenu_Paint;

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            BringToFront();
            Focus();
            WindowManager.BringWindowToFront(Handle);
        }


        private void RadialMenu_Load(object sender, EventArgs e)
        {
            var options = AppSettings.Load<RadialOptions>("RadialOptions");
            if (options == null)
            {
                options = GetDefaultRadialOptions();
            }

            SetRadialOptions(options);            
        }
        // Close
        private void RadialMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            isMenuOpen = false;
        }



        // Paint
        private void RadialButtonMenu_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Calculate the center of the form
            Point formCenter = new Point(this.Width / 2, this.Height / 2);

            // Draw the dead zone as a filled circle
            g.FillEllipse(Brushes.Red, formCenter.X - DeadZoneRadius, formCenter.Y - DeadZoneRadius, DeadZoneRadius * 2, DeadZoneRadius * 2);

            // Draw the radial menu as a series of slices
            for (int i = 0; i < 4; i++)
            {
                int startAngle = i * 90;
                int sweepAngle = 90;

                if (i == activeSlice)
                {
                    // Display visual feedback based on the activeSlice
                    g.FillPie(Brushes.Black, formCenter.X - MenuRadius, formCenter.Y - MenuRadius, MenuRadius * 2, MenuRadius * 2, startAngle, sweepAngle);
                }
                else
                {
                    g.DrawPie(Pens.White, formCenter.X - MenuRadius, formCenter.Y - MenuRadius, MenuRadius * 2, MenuRadius * 2, startAngle, sweepAngle);
                }
            }
        }



        // Radial Options
        private void SetRadialOptions(RadialOptions radialOptions)
        {
            ClearRadialOptions();

            int panelIndex = 1;
            foreach(RadialOption radialOption in radialOptions.Options)
            {
                foreach (AControl aControl in radialOption.Controls)
                {
                    Control optionControl = ControlFactory.CreateControl(aControl.Type);
                    optionControl = aControl.CopyProperties(optionControl);

                    string panelName = "panel" + panelIndex;
                    Panel targetPanel = this.Controls.Find(panelName, true).FirstOrDefault() as Panel;

                    if (targetPanel != null)
                    {
                        InvokeMethod(aControl.Action, optionControl);
                        targetPanel.Controls.Add(optionControl);
                    }
                }

                panelIndex++;
            }
        }
        private void ClearRadialOptions()
        {
            panel1.Controls.Clear();
            panel2.Controls.Clear();
            panel3.Controls.Clear();
            panel4.Controls.Clear();
        }
        private RadialOptions GetDefaultRadialOptions()
        {
            var options = new RadialOptions();
            
            // Option 1
            var defaultRadialOption = new RadialOption();
            var aControl = new AControl();
            aControl.Size = new Size(100, 21);
            aControl.Location = new Point(0, 0);            
            aControl.Type = "ComboBox";
            aControl.Action = RadialAction.Actions.ChangeDefaultAudioPlayback;
            defaultRadialOption.Controls.Add(aControl);            
            options.Options.Add(defaultRadialOption);

            // Option 2
            defaultRadialOption = new RadialOption();
            aControl = new AControl();
            aControl.Size = new Size(100, 21);
            aControl.Location = new Point(panel2.Width - aControl.Size.Width, 0);            
            aControl.Type = "ComboBox";
            aControl.Action = RadialAction.Actions.ChangeDefaultAudioMic;
            defaultRadialOption.Controls.Add(aControl);            
            options.Options.Add(defaultRadialOption);

            // Option 3
            defaultRadialOption = new RadialOption();
            aControl = new AControl();
            aControl.Size = new Size(75, 21);
            aControl.Location = new Point(panel3.Width - aControl.Size.Width, panel3.Height - aControl.Size.Height);            
            aControl.Type = "CheckBox";
            aControl.Text = "Auto Run";
            aControl.Action = RadialAction.Actions.AutoRun;
            defaultRadialOption.Controls.Add(aControl);            
            options.Options.Add(defaultRadialOption);

            // Option 4
            defaultRadialOption = new RadialOption();
            aControl = new AControl();
            aControl.Size = new Size(75, 25);
            aControl.Location = new Point(0, panel3.Height - aControl.Size.Height);
            aControl.Type = "Button";
            aControl.Text = "Edge";
            aControl.Action = RadialAction.Actions.OpenApp;
            aControl.Tag = "https://www.google.com";
            defaultRadialOption.Controls.Add(aControl);            
            options.Options.Add(defaultRadialOption);

            return options;
        }
        public void InvokeMethod(Actions action, Control optionControl)
        {
            string methodName = Enum.GetName(typeof(Actions), action);
            var methodInfo = this.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (methodInfo != null)
            {
                methodInfo.Invoke(this, new object[] { optionControl });
            }
        }




        // Actions

        // Audio
        private void ChangeDefaultAudioPlayback(Control control)
        {
            ComboBox comboBox = control as ComboBox;
            if (comboBox == null) return;

            comboBox.Items.Clear();

            audioDeviceManager = new AudioDeviceManager();
            var devices = audioDeviceManager.GetAudioPlaybackDevices();
            var defaultDevice = audioDeviceManager.GetDefaultAudioPlaybackDevice();            

            int index = 0;
            int defaultDeviceIndex = 0;            
            foreach (var device in devices)
            {
                comboBox.Items.Add(device);

                if (device.ID == defaultDevice.ID)
                    defaultDeviceIndex = index;

                index++;
            }

            comboBox.SelectedIndex = defaultDeviceIndex;
            comboBox.SelectedIndexChanged += (senderr, ee) =>
            {
                ComboBox combobox = senderr as ComboBox;

                if (combobox.SelectedItem != null)
                {
                    MMDevice selectedDevice = (MMDevice)combobox.SelectedItem;

                    try
                    {
                        audioDeviceManager.SetDefaultAudioPlaybackDevice(selectedDevice);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            };
        }
        private void ChangeDefaultAudioMic(Control control)
        {
            ComboBox comboBox = control as ComboBox;
            if (comboBox == null) return;

            comboBox.Items.Clear();

            audioDeviceManager = new AudioDeviceManager();
            var devices = audioDeviceManager.GetAudioRecordingDevices();
            var defaultDevice = audioDeviceManager.GetDefaultAudioRecordingDevice();

            int index = 0;
            int defaultDeviceIndex = 0;
            foreach (var device in devices)
            {
                comboBox.Items.Add(device);

                if (device.ID == defaultDevice.ID)
                    defaultDeviceIndex = index;

                index++;
            }

            comboBox.SelectedIndex = defaultDeviceIndex;
            comboBox.SelectedIndexChanged += (senderr, ee) =>
            {
                ComboBox combobox = senderr as ComboBox;

                if (combobox.SelectedItem != null)
                {
                    MMDevice selectedDevice = (MMDevice)combobox.SelectedItem;

                    try
                    {
                        audioDeviceManager.SetDefaultAudioRecordingDevice(selectedDevice);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            };
        }


        // Apps
        private void OpenApp(Control control)
        {
            Button button = control as Button;
            if (button == null) return;

            button.Click += (sender, e) =>
            {
                if (control.Tag != null && control.Tag is string processName)
                {
                    try
                    {
                        Process.Start(processName);
                        Close();
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception if needed
                        notificationManager.ShowNotification($"Error starting app {processName}: {ex.Message}");
                    }
                }
            };
        }


        // Auto Run
        private void AutoRun(Control control)
        {
            CheckBox checkBox = control as CheckBox;
            if (checkBox == null) return;

            checkBox.Checked = macroActions.AutoRun;

            checkBox.CheckedChanged += (sender, e) =>
            {
                CheckBox checkbox = sender as CheckBox;
                if(checkbox.Checked)
                {
                    // Start auto running
                    macroActions.AutoRun = true;
                    notificationManager.ShowNotification("Auto Run Enabled");

                    // Alt+Tab to prev. app
                    Keyboard.PressKeyDown(0x12);
                    Keyboard.PressKeyDown((byte)Keys.Tab);
                    Thread.Sleep(50);
                    Keyboard.PressKeyUp(0x12);
                    Keyboard.PressKeyUp((byte)Keys.Tab);

                    Thread.Sleep(500);
                    Task.Run(() =>
                    {
                        while (macroActions.AutoRun)
                        {
                            Keyboard.PressKeyDown((byte)Keys.LShiftKey);
                            Keyboard.PressKeyDown((byte)(Keys.W));
                            Thread.Sleep(50);
                            Keyboard.PressKeyUp((byte)(Keys.W));
                            Keyboard.PressKeyUp((byte)Keys.LShiftKey);
                            Thread.Sleep(50);
                        }
                    });
                }
                else
                {
                    // Stop auto running
                    macroActions.AutoRun = false; 
                    notificationManager.ShowNotification("Auto Run Disabled");
                    Keyboard.PressKeyUp((byte)(Keys.W));
                    Keyboard.PressKeyUp((byte)Keys.LShiftKey);
                }

                Close();
            };
        }



        // Mouse move/down
        private void RadialButtonMenu_MouseMove(object sender, MouseEventArgs e)
        {
            int distance = (int)Math.Sqrt(Math.Pow(e.X - formCenter.X, 2) + Math.Pow(e.Y - formCenter.Y, 2));

            if (distance < DeadZoneRadius)
            {
                activeSlice = -1; // Inside the dead zone
                ShowSlice(-1);
            }
            else
            {
                int angle = GetAngle(e.Location, formCenter);

                int sliceIndex = angle / 90;
                activeSlice = sliceIndex;

                ShowSlice(sliceIndex);
            }

            this.Invalidate(); // Trigger a repaint
        }
        private void RadialButtonMenu_MouseDown(object sender, MouseEventArgs e)
        {
            int distance = (int)Math.Sqrt(Math.Pow(e.X - formCenter.X, 2) + Math.Pow(e.Y - formCenter.Y, 2));

            if (distance < DeadZoneRadius)
            {
                // Click inside the dead zone
                HandleSliceClick(-1);
            }
            else
            {
                int angle = GetAngle(e.Location, formCenter);
                int sliceIndex = angle / 90;

                if (sliceIndex == activeSlice)
                {
                    HandleSliceClick(sliceIndex);
                }
            }
        }



        // Slice Selection
        private void ShowSlice(int sliceIndex)
        {
            if (panel1 == null) return;
            switch (sliceIndex)
            {
                case 0:
                    panel1.Visible = true;
                    panel2.Visible = false;
                    panel3.Visible = false;
                    panel4.Visible = false;
                    break;
                case 1:
                    panel1.Visible = false;
                    panel2.Visible = true;
                    panel3.Visible = false;
                    panel4.Visible = false;
                    break;
                case 2:
                    panel1.Visible = false;
                    panel2.Visible = false;
                    panel3.Visible = true;
                    panel4.Visible = false;
                    break;
                case 3:
                    panel1.Visible = false;
                    panel2.Visible = false;
                    panel3.Visible = false;
                    panel4.Visible = true;
                    break;
                default:
                    panel1.Visible = false;
                    panel2.Visible = false;
                    panel3.Visible = false;
                    panel4.Visible = false;
                    // in Dead Zone
                    break;
            }
        }



        // Slice Clicks
        private void HandleSlice1Click()
        {
            var tabs = ProcessManager.GetOpenTabsWithText("Home");
            if (tabs.Count > 0)
            {
                // Bring to front
                WindowManager.BringWindowToFront(tabs[0]);
            }
            else
            {
                // Launch browser
            }
            this.Close();
        }
        private void HandleSlice2Click()
        {
            this.Close();
        }
        private void HandleSlice3Click()
        {
            this.Close();
        }
        private void HandleSlice4Click()
        {
            this.Close();
        }
        private void HandleSliceClick(int sliceIndex)
        {
            switch (sliceIndex)
            {
                case 0:
                    HandleSlice1Click();
                    break;
                case 1:
                    HandleSlice2Click();
                    break;
                case 2:
                    HandleSlice3Click();
                    break;
                case 3:
                    HandleSlice4Click();
                    break;
                default:
                    this.Close(); // Clicked in Dead Zone
                    break;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            HandleSlice1Click();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            HandleSlice2Click();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            HandleSlice3Click();
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            HandleSlice4Click();
        }



        // Helpers
        private int GetAngle(Point point, Point center)
        {
            int deltaX = point.X - center.X;
            int deltaY = point.Y - center.Y;

            double radians = Math.Atan2(deltaY, deltaX);
            double angle = radians * (180 / Math.PI);

            if (angle < 0)
            {
                angle = 360 + angle;
            }

            return (int)angle;
        }
    }


    public class RadialAction
    {
        public enum Actions
        {
            ChangeDefaultAudioPlayback,
            ChangeDefaultAudioMic,
            OpenApp,
            AutoRun,
        }
    }
    internal class RadialOptions
    {
        public List<RadialOption> Options { get; set; }
        public RadialOptions() 
        {
            Options = new List<RadialOption>();
        }
    }
    internal class RadialOption
    {
        public string Name { get; set; }
        public List<AControl> Controls { get; set; }

        public RadialOption()
        {
            Name = "";
            Controls = new List<AControl>();
        }
    }
    internal class AControl
    {
        public RadialAction.Actions Action { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public object Tag { get; set; }
        public Point Location { get; set; }
        public Size Size { get; set; }
        public object Value { get; set; }
        public List<object> Items { get; set; }
        public string Text { get; set; }
        public Image Image { get; set; }
        public Image BackgroundImage { get; set; }
        public Color Color { get; set; }
        public Color BackgroundColor { get; set; }
        public int TextSize { get; set; }


        internal Control CopyProperties(Control optionControl)
        {
            foreach (var property in typeof(AControl).GetProperties())
            {
                // Check if the property exists in the actual control
                var controlProperty = optionControl.GetType().GetProperty(property.Name);

                if (controlProperty != null)
                {
                    // Check if the control property has a getter and setter
                    if (controlProperty.CanRead && controlProperty.CanWrite)
                    {
                        // Copy the value from the AControl property to the control property
                        controlProperty.SetValue(optionControl, property.GetValue(this));
                    }
                }
            }

            return optionControl; // Return the modified instance
        }
    }


    public class ControlFactory
    {
        private static readonly Dictionary<string, Func<Control>> ControlTypeMap = new Dictionary<string, Func<Control>>
        {
            { "Button", () => new Button() },
            { "TextBox", () => new TextBox() },
            { "CheckBox", () => new CheckBox() },
            { "ComboBox", () => new ComboBox() },
            { "RadioButton", () => new RadioButton() },
        };

        public static Control CreateControl(string controlTypeName)
        {
            if (ControlTypeMap.TryGetValue(controlTypeName, out var controlFactory))
            {
                return controlFactory.Invoke();
            }

            throw new ArgumentException("Invalid control type", nameof(controlTypeName));
        }
    }
}
