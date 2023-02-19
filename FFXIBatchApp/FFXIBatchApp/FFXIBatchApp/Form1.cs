using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FFXIBatchApp
{
	public partial class Form1 : Form
	{
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string className, string windowName);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        Thread ActiveExtractThread;

        public Form1()
		{
			InitializeComponent();
		}

        private void Form1_Load(object sender, EventArgs e)
		{
			// Set log file
			Logger.SetLogFile();

            // Load settings
            LoadSettings();

			// Subscribe to hook listeners
			HookSubscribe();

            if (Settings.GetSetting("PathFFXI").Length == 0)
            {
                ConsoleLog("!!! PLEASE CLICK THE SETTINGS TAB AND SELECT YOUR FFXI PATH !!!");
            }

            // Perform Altana List Conversion and then enable the UI
            CheckAltanaData();

			// Introduction
			ConsoleLog("-----------------------------");
			ConsoleLog("FFXI Batch Exporter v1.0");
			ConsoleLog("Built by: Vekien");
			ConsoleLog("- Please read the about tab...");
			ConsoleLog("- App Ready!");
			ConsoleLog("-----------------------------");

			// Quick check for Noesis
			CheckForNoesis();
		}

        private void Form1_Unload(object sender, FormClosingEventArgs e)
        {
            // Remove hook listeners
            HookUnsubscribe();
        }

        private void LoadSettings()
        {
            PathFFXI.Text = Settings.GetSetting("PathFFXI");
		}

		/// <summary>
		/// Check we have all AltanaViewer data converted
		/// </summary>
		private void CheckAltanaData()
        {
            AltanaListConverter converter = new AltanaListConverter();
            converter.init();
		}

        /// <summary>
        /// Simple look for Noesis as I'm too lazy to handle a state where
        /// it is not open!
        /// </summary>
        private bool CheckForNoesis()
        {
            Process[] pname = Process.GetProcessesByName("Noesis64");

            if (pname.Length == 0)
            {
                ConsoleLog($"Please start Noesis64.exe before running any actions!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Basic Console Log method
        /// </summary>
        /// <param name="message"></param>
        private void ConsoleLog(string message)
        {
            Logger.Add(message);
        }

        /// <summary>
        /// My lush log timer lol Populates the log box, can reduce friction and 
        /// also means the log writes to a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                foreach (string entry in Logger.Get())
                {
                    //LogBox.SelectionStart = 0;
                    // LogBox.SelectionLength = 0;
                    LogBox.SelectedText = entry + Environment.NewLine;

                    // set the current caret position to the end
                    LogBox.SelectionStart = LogBox.Text.Length;

                    // scroll it automatically
                    LogBox.ScrollToCaret();
                }

                Logger.Clear();
            }
            catch { }
        }

        /// <summary>
        /// Send a key to a specific window title.
        /// Default to Noesis.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="wait"></param>
        /// <param name="window"></param>
        private void SendKey(string key, int wait = 400, string window = "Noesis")
        {
            try
            {
                IntPtr hWnd = FindWindow(null, window);
                SetForegroundWindow(hWnd);

                // send the key
                ConsoleLog($">>(SendKey) Key: {key}");
                SendKeys.SendWait(key);

                Thread.Sleep(wait);
            }
            catch (Exception Ex)
            {
                ConsoleLog($"!!! (SendKey) Exception Thrown: {Ex.Message}");
            }
        }

        /// <summary>
        /// Sends the tab key multiple times, hard coded to "Export Media" as that
        /// is usually the only window that needs this logic...
        /// </summary>
        /// <param name="count"></param>
        /// <param name="window"></param>
        private void SendKeyTabN(int count, string window = "Export Media")
        {
            for (int i = 1; i <= count; i++)
            {
                SendKey("{Tab}", 250, "Export Media");
            }
        }

        /// <summary>
        /// Stops the current active thread
        /// </summary>
        private void StopActiveThread()
        {
            if (ActiveExtractThread == null)
            {
                return;
            }

            ConsoleLog($"Stopping active extraction...");
            ActiveExtractThread.Abort();
            ConsoleLog(">> Active extract stopped!");
        }

        /// <summary>
        /// Wait for the active window to be said windowTitle, improves timing of send key flows.
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        private bool WaitForActiveWindow(string windowTitle)
        {
            ConsoleLog($"Waiting for window: {windowTitle} ...");

            for (int i = 0; i <= 15; i++)
            {
                if (GetActiveWindowTitle() == windowTitle)
                {
                    return true;
                }

                Thread.Sleep(200);
            }

            ConsoleLog($"!! Error: Could not detect the Window: {windowTitle} - Current active window: {GetActiveWindowTitle()}");

            return false;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/115868/how-do-i-get-the-title-of-the-current-active-window-using-c
        /// </summary>
        /// <returns></returns>
        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        /// <summary>
        /// Discord for FF11 Modding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FFXIModdingDiscordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://discord.gg/nHESRq3fHu");
        }

        /// <summary>
        /// Link to github
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GithubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/vekien/xidata");
        }

        /// -----------------------------------------------------------------------------------------
        /// GUI Buttons
        /// -----------------------------------------------------------------------------------------

        /// <summary>
        /// Stop all extractions, aborts the current thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopEverything_Click(object sender, EventArgs e)
        {
            StopActiveThread();
        }

        /// <summary>
        /// Start the NPC Extract in STA Threading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [STAThread]
        private void StartNpcExtract_Click(object sender, EventArgs e)
		{
            if (!CheckForNoesis()) { return; }

            ConsoleLog($"Start NPC Extract...");

            ActiveExtractThread = new Thread(() => HandleSingleNpcExtract());
            ActiveExtractThread.SetApartmentState(ApartmentState.STA);
            ActiveExtractThread.Start();
        }

        /// -----------------------------------------------------------------------------------------
        /// Send Key Logic Flows
        /// -----------------------------------------------------------------------------------------

        /// <summary>
        /// All the crazy SendKey logic for handling a Single NPC Extract
        /// </summary>
        private void HandleSingleNpcExtract()
        {
            // dat 
            // todo - this should read from a list.
            string dat = "E:\\SquareEnix\\SquareEnix\\FINAL FANTASY XI\\ROM\\250\\79.DAT";
            Clipboard.SetText(dat);

            // Open a DAT
            SendKey("%f", 400);
            SendKey("o", 200);

            Thread.Sleep(5000);

            if (WaitForActiveWindow("Open"))
            {
                // Paste in the dat and press Enter, then give it a bit of time to load (usually is fast)
                SendKey("^v", 500, "Open");
                SendKey("{ENTER}", 1200, "Open");

                // Now open the Export Window
                SendKey("%f", 400);
                SendKey("e", 400);

                // Wait for the Export Media window to open
                if (WaitForActiveWindow("Export Media"))
                {
                    SendKeyTabN(3); // Destination
 
                }
            }
        }

        /// -----------------------------------------------------------------------------------------
        /// Escape Key Listener
        /// - This listens for "Escape" key
        /// It uses: https://github.com/gmamaladze/globalmousekeyhook
        /// -----------------------------------------------------------------------------------------

        private IKeyboardMouseEvents m_GlobalHook;
        private void HookSubscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        private void HookUnsubscribe()
        {
            // m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            // It is recommened to dispose it
            m_GlobalHook.Dispose();
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            string character = e.KeyChar.ToString();

            // fix some
            character = ((sbyte)e.KeyChar == 13) ? "ENTER" : character;
            character = ((sbyte)e.KeyChar == 32) ? "SPACE" : character;

            // ConsoleLog($"Key: {(sbyte)e.KeyChar} == {character}");

            // Escape
            if ((sbyte)e.KeyChar == 27)
            {
                ConsoleLog($"[HOOK] (Escape Key detected!)");
                StopActiveThread();
                return;
            }
        }

		/// ----------------------------------------------------------------------------------------- 
        /// Select FFXI Path
        /// -----------------------------------------------------------------------------------------

		private void PathFFXI_SelectPath(object sender, MouseEventArgs e)
		{
			var dialog = new FolderBrowserDialog();
            dialog.Description = "Browse To: SquareEnix\\FINAL FANTASY XI";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				PathFFXI.Text = dialog.SelectedPath;
                Settings.SaveSetting("PathFFXI", PathFFXI.Text);
			}
		}
	}
}

[StructLayout(LayoutKind.Sequential)]
internal struct RECT
{
	public int Left;
	public int Top;
	public int Right;
	public int Bottom;
}