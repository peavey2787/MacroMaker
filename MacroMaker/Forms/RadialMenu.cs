using System;
using System.Drawing;
using System.Windows.Forms;
using MacroMaker.Classes;

namespace MacroMaker.Forms
{
    public partial class RadialMenu : Form
    {
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

        public RadialMenu(Point mouseLocation)
        {
            InitializeComponent();

            // Check if the menu is already open, and if so, close the new instance
            if (isMenuOpen)
            {
                this.Close();
                return;
            }

            // If not, mark the menu as open and proceed with initialization
            isMenuOpen = true;
            
            this.centerLocation = mouseLocation;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            InitializeRadialMenu();
        }

        private void InitializeRadialMenu()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;

            // Set the size of the form to 400x400 pixels
            this.Size = new Size(400, 400);

            // Calculate the location to center the form based on centerLocation
            int x = (centerLocation.X - this.Width / 2) - DeadZoneRadius;
            int y = (centerLocation.Y - this.Height / 2) - DeadZoneRadius;

            this.Location = new Point(x, y);

            // Set the background color to the desired color
            this.BackColor = Color.Beige;

            // Make the background color transparent
            this.TransparencyKey = Color.Beige;

            this.MouseMove += RadialButtonMenu_MouseMove;
            this.MouseDown += RadialButtonMenu_MouseDown;
            this.Paint += RadialButtonMenu_Paint;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            BringToFront();
            Focus();
        }


        private void RadialMenu_Load(object sender, EventArgs e)
        {
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


}
