using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemTrayApp.Properties;

namespace SystemTrayApp
{
	/// <summary>
	/// 
	/// </summary>
	class ProcessIcon : IDisposable
	{
		/// <summary>
		/// The NotifyIcon object.
		/// </summary>
		NotifyIcon ni;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessIcon"/> class.
		/// </summary>
		public ProcessIcon()
		{
			// Instantiate the NotifyIcon object.
			ni = new NotifyIcon();
		}

		/// <summary>
		/// Displays the icon in the system tray.
		/// </summary>
		public void Display()
		{
			// Put the icon in the system tray and allow it react to mouse clicks.			
			ni.MouseClick += new MouseEventHandler(ni_MouseClick);
			ni.Icon = Resources.SystemTrayApp;
			ni.Text = "System Tray Utility Application Demonstration Program";
			ni.Visible = true;

			// Attach a context menu.
			ni.ContextMenuStrip = new ContextMenus().Create();

            Task.Factory.StartNew(() => WordReminder()); 
		}

        private void WordReminder()
        {
            try
            {
                List<string> data = File.ReadAllLines(@"./data.csv").ToList();
                if (data.Count == 0)
                    return;

                string whiteSpace = " ";
                while (true)
                {
                    Random ran = new Random();
                    int index = ran.Next(0, data.Count);
                    string[] info = data[index].Split(new string[] { "," }, StringSplitOptions.None);
                    if (info.Length < 2)
                        continue;

                    string[] meaningInfo = info[1].Split(new string[] { whiteSpace }, StringSplitOptions.None);
                    string amHan = meaningInfo[0];
                    string nghiaTiengViet = whiteSpace;
                    for (int i = 1; i < meaningInfo.Length; i++)
                    {
                        nghiaTiengViet += meaningInfo[i] + whiteSpace;
                    }
                    NotifyIcon notifyIcon1 = new NotifyIcon();
                    notifyIcon1.Visible = true;
                    notifyIcon1.Icon = SystemIcons.Question;
                    notifyIcon1.BalloonTipTitle = info[0] + ": " + amHan;
                    notifyIcon1.BalloonTipText = nghiaTiengViet;
                    notifyIcon1.Text = "Try your best Toan-san";
                    notifyIcon1.BalloonTipClicked += new EventHandler(notifyIcon_BalloonTipClicked);
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.None;
                    notifyIcon1.ShowBalloonTip(20000);
                    
                    Thread.Sleep(60000);
                    notifyIcon1.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            Process.Start("www.gooogle.com");
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
		{
			// When the application closes, this will remove the icon from the system tray immediately.
			ni.Dispose();
		}

		/// <summary>
		/// Handles the MouseClick event of the ni control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		void ni_MouseClick(object sender, MouseEventArgs e)
		{
			// Handle mouse button clicks.
			if (e.Button == MouseButtons.Left)
			{
				// Start Windows Explorer.
				Process.Start("explorer", null);
			}
		}
	}
}