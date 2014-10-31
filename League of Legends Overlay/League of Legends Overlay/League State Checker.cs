using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace League_of_Legends_Overlay
{
    class League_State_Checker
    {

        #region Getting League Window Size / Location
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);
        #endregion

        #region Global Variables 
        Form2 overlayForm = new Form2();
        Thread stateCheckerThread;
        Process leagueProcess;
        RECT lRect;
        #endregion

        /// <summary>
        /// Constructs the league state checker thread to run in the background
        /// </summary>
        public League_State_Checker()
        {
            stateCheckerThread = new Thread(new ThreadStart(stateChecker));
        }

        /// <summary>
        /// Starts the league state checker thread
        /// </summary>
        public void start()
        {
            overlayForm.Show();
            //overlayForm.Visible = false;
            //stateCheckerThread.Start();
        }

        /// <summary>
        /// Stops the league state checker thread
        /// </summary>
        public void stop()
        {
            stateCheckerThread.Abort();
        }

        /// <summary>
        /// Checks all open processes for the league of legends window
        /// </summary>
        /// <returns> True if window is open, false if not</returns>
        public bool leagueWindowOpen()
        {
            Process[] processlist = Process.GetProcesses();
            Process lProcess = null;
            foreach (Process process in processlist)
            {
                if ((process.MainWindowTitle).ToString().Equals("League of Legends (TM) Client"))
                {
                   GetWindowRect(process.Handle, ref lRect);
                   lProcess = process;
                   leagueProcess = process;
                }
            }
            return lProcess != null;
        }

        /// <summary>
        /// Reads the most recent log file. If the log file does not contain the statement
        /// that notifies that the game is starting, then return true
        /// </summary>
        /// <returns>Returns true if the game is open but on load screen</returns>
        public bool loadScreenOpen()
        {
            DirectoryInfo logDirInf = new DirectoryInfo("C:\\Riot Games\\League of Legends\\Logs\\Game - R3d Logs");
            StreamReader streamReader;
            string str;
            Stream stream;

            var myFile = logDirInf.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
            stream = File.Open(myFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            streamReader = new StreamReader(stream);
            str = streamReader.ReadToEnd();

            return (!(str.Contains("GAMESTATE_GAMELOOP Begin")));
        }

        //load screen overlay displayer
        public void displayLoadScreenOverlay()
        {
            
            overlayForm.Invoke((MethodInvoker)delegate
            {
                overlayForm.Location = new Point(lRect.left,lRect.top);
                overlayForm.Height = lRect.bottom;
                overlayForm.Width = lRect.right;
                overlayForm.Visible = true;
                overlayForm.overlayPictureBox.Image = League_of_Legends_Overlay.Properties.Resources.load_screen_overlay;
            });
        }

        //Handles the picturebox manipulation in form2 (using standard overlay)
        public void displayInGameOverlay()
        {
            overlayForm.Invoke((MethodInvoker)delegate
            {
                overlayForm.Location = new Point(lRect.left, lRect.top);
                overlayForm.Height = lRect.bottom;
                overlayForm.Width = lRect.right;
                overlayForm.Visible = true;
                overlayForm.overlayPictureBox.Image = League_of_Legends_Overlay.Properties.Resources.diamond_overlay;
            });
        }

        /// <summary>
        /// This is what will continuously happen while the state checker thread
        /// is active. ths logic main branch.
        /// </summary>
        private void stateChecker()
        {
            while(stateCheckerThread.IsAlive)
            {
                if (leagueWindowOpen())
                {                    
                    if (loadScreenOpen())
                    {
                        displayLoadScreenOverlay();
                    }
                    else
                    {
                        displayInGameOverlay();
                    }
                }
                else
                {
                    overlayForm.Invoke((MethodInvoker)delegate
                    {
                        overlayForm.Visible = false;
                    });
                }
                Thread.Sleep(1000);
            }

        }


    }
}
