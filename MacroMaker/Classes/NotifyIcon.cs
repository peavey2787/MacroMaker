using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroMaker.Classes
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class NotificationManager
    {
        private NotifyIcon notifyIcon;

        public NotificationManager()
        {
            InitializeNotifyIcon();
        }

        private void InitializeNotifyIcon()
        {
            // Create NotifyIcon instance
            notifyIcon = new NotifyIcon
            {
                Visible = true,
                Icon = SystemIcons.Information,
                BalloonTipIcon = ToolTipIcon.Info
            };
        }

        public void ShowNotification(string message, string title = "Notification")
        {
            // Display the notification
            notifyIcon.ShowBalloonTip(3000, title, message, ToolTipIcon.Info);
        }
    }
}
