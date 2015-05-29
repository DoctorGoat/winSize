using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace winSize
{
    //winSize is a tool for resizing another window.
    //The games Diablo 3 and Guild Wars 2 both have windowed mode,
    //but no support for setting the window to an arbitrary size.
    //Thus, this tool was created.


    public partial class Form1 : Form
    {
        //initialize, refresh available windows
        //Does not get current window for some reason?
        public Form1()
        {
            InitializeComponent();
            getTitles();
        }

        //Import functions MoveWindow and FindWindow from the Windows API
        //TODO: Get window bounds so window doesn't move when resized
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //Clear the item list. Then, get a list of processes and use the process.MainWindowTitle 
        //list to populate the window list. Select the first item.
        private void getTitles()
        {
            winList.Items.Clear();
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (process.MainWindowTitle != "")
                {
                    winList.Items.Add(process.MainWindowTitle);
                }
            }
            winList.SelectedItem = winList.Items[0];
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            getTitles();
        }

        //Find the window with the desired title.
        //Then, call MoveWindow and set its x and y values to
        //the textbox's specifications.
        private void resizeButton_Click(object sender, EventArgs e)
        {
            int xValue = 0;
            int yValue = 0;
            try
            {
                xValue = Convert.ToInt32(textBox1.Text);
                yValue = Convert.ToInt32(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Invalid x/y sizes.");
            }
            try
            {
                if (xValue > 0 && yValue > 0)
                {
                    IntPtr thisWin = FindWindow(null, winList.Text);
                    MoveWindow(thisWin, 0, 0, xValue, yValue, true);
                }
            }
            catch
            {
                Console.WriteLine("Could not resize window:" + winList.Text);
            }
        }

        //Same as above, but sets the window's resolution to the screen's resolution. 
        //Also happens if you try to set a window to larger than the screen resolution above.

        //TODO: Possibly remove the borders to make an implementation of borderless fullscreen?
        private void resButton_Click(object sender, EventArgs e)
        {
            try
            {
                IntPtr thisWin = FindWindow(null, winList.Text);
                MoveWindow(thisWin, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, true);
            }
            catch
            {
                MessageBox.Show("Could not resize window:" + winList.Text);
            }

        }
    }


}
