using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        bool ActiveThreadRunning = false;

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

			// Build local data
			(new AltanaListConverter()).init();
			(new NoesisConverter()).init();

			// Introduction
			ConsoleLog("-----------------------------");
			ConsoleLog("FFXI Batch Exporter v1.0");
			ConsoleLog("> Built by: Vekien");
            ConsoleLog("> Please set your settings if you have not done so already.");
			ConsoleLog("> App Ready!");
			ConsoleLog("-----------------------------");
		}

        private void Form1_Unload(object sender, FormClosingEventArgs e)
        {
            // Remove hook listeners
            HookUnsubscribe();
        }

        private void LoadSettings()
        {
            PathFFXI.Text = Settings.GetSetting("PathFFXI");
            PathNoesis.Text = Settings.GetSetting("PathNoesis");
		}

        /// <summary>
        /// Check if Noesis is open, if not, open it :D
        /// </summary>
        private bool CheckForNoesis()
        {
            Process[] pname = Process.GetProcessesByName("Noesis64");

            if (pname.Length == 0)
            {
                string noesisFolder = Settings.GetSetting("PathNoesis");

                if (noesisFolder.Length == 0) 
                {
                    ConsoleLog("Error: Please set your Noesis Folder in the settings...");
                    return false;
                }

                // todo - this should just open it.
                ConsoleLog("Launching Noesis!");
                string noesisFilepath = $"{noesisFolder}\\Noesis64.exe";
			    Process.Start(noesisFilepath);

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
                //ConsoleLog($">>(SendKey) Key: {key}");
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
			ActiveThreadRunning = false;

			if (ActiveExtractThread == null)
            {
                return;
            }

            ConsoleLog($"Stopping active extraction...");

            ActiveExtractThread.Abort();

            ToggleButtons();

			ConsoleLog(">> Active extract stopped!");
        }

        /// <summary>
        /// Wait for the active window to be said windowTitle, improves timing of send key flows.
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        private bool WaitForActiveWindow(string windowTitle)
        {
            //ConsoleLog($"Waiting for window: {windowTitle} ...");

            // Will wait for a max of 30 seconds.
            for (int i = 0; i <= 60; i++)
            {
                if (GetActiveWindowTitle() == windowTitle)
                {
                    return true;
                }

                Thread.Sleep(500);
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
        /// Run a Noesis ?cmode Command.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="idleStateCode"></param>
		private void RunNoesisCMode(string arguments, int idleStateCode = 0)
		{
			// Grab Noesis Path
			string noesisPath = Settings.GetSetting("PathNoesis");

			var proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = noesisPath,
					Arguments = $"?cmode {arguments}",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
			        CreateNoWindow = false,
					WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
				}
			};

			proc.Start();

            // Run the command slightly different based on idle state codes.
            switch(idleStateCode)
            {
                default:
                    ConsoleLog($"!! Error: Unknown idle state provided: {idleStateCode}");
                    break;

                // normal, just wait for the process to complete
                case 0:
					proc.WaitForExit();
                    break;

                // Look for an "Open" dialog and then close it.
                case 1:
					if (WaitForActiveWindow("Open"))
					{
                        // close it
						SendKey("%{F4}", 100, "Open");

						// Wait for command to finish
						proc.WaitForExit();
					}

                    break;

                // Look for an "Open" dialog and enter in the skeleton
                case 2:
					Clipboard.SetText(ExtractSkeleton);

					if (WaitForActiveWindow("Open"))
					{
						// Paste in the dat and press Enter, then give it a bit of time to load (usually is fast)
						SendKey("^v", 200, "Open");
						SendKey("{ENTER}", 600, "Open");

						// Wait for command to finish
						proc.WaitForExit();
					}

                    break;
			}
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

        [STAThread]
        private void StartNpcExtract_Click(object sender, EventArgs e)
		{
            if (!CheckForNoesis()) { return; }
            if (ActiveThreadRunning) { return; }

            ActiveExtractThread = new Thread(() => HandleNpcMainExtract());
            ActiveExtractThread.SetApartmentState(ApartmentState.STA);
            ActiveExtractThread.Start();

			ActiveThreadRunning = true;
            ToggleButtons();
		}

		[STAThread]
		private void StartZoneExtract_Click(object sender, EventArgs e)
		{
			if (ActiveThreadRunning) { return; }

			ActiveExtractThread = new Thread(() => HandleZonesExtract());
			ActiveExtractThread.SetApartmentState(ApartmentState.STA);
			ActiveExtractThread.Start();

			ActiveThreadRunning = true;
			ToggleButtons();
		}

		[STAThread]
		private void StartWeaponsExtract_Click(object sender, EventArgs e)
		{
			if (ActiveThreadRunning) { return; }

			ActiveExtractThread = new Thread(() => HandleWeaponsExtract());
			ActiveExtractThread.SetApartmentState(ApartmentState.STA);
			ActiveExtractThread.Start();

			ActiveThreadRunning = true;
			ToggleButtons();
		}

		[STAThread]
		private void StartArmorExtract_Click(object sender, EventArgs e)
		{
			if (ActiveThreadRunning) { return; }

			ActiveExtractThread = new Thread(() => HandleArmorExtract());
			ActiveExtractThread.SetApartmentState(ApartmentState.STA);
			ActiveExtractThread.Start();

			ActiveThreadRunning = true;
			ToggleButtons();
		}

		[STAThread]
		private void StartAnimationsExtract_Click(object sender, EventArgs e)
		{
			if (ActiveThreadRunning) { return; }

			ActiveExtractThread = new Thread(() => HandleArmorExtract());
			ActiveExtractThread.SetApartmentState(ApartmentState.STA);
			ActiveExtractThread.Start();

			ActiveThreadRunning = true;
			ToggleButtons();
		}

		/// <summary>
		/// Toggle the state of buttons based on if a thread is running or not.
		/// </summary>
		private void ToggleButtons()
        {
            StartZoneExtract.Enabled = !ActiveThreadRunning;
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
        /// Settings
        /// -----------------------------------------------------------------------------------------

		private void PathFFXI_SelectPath(object sender, MouseEventArgs e)
		{
			var dialog = new FolderBrowserDialog();
            dialog.Description = "Browse To: SquareEnix\\FINAL FANTASY XI";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string selectedPath = dialog.SelectedPath;
				string checkFilename = $"{selectedPath}\\FFXiMain.dll";
				
				// Check noesis exists in the selected path
				if (File.Exists(checkFilename))
				{
					PathFFXI.Text = selectedPath;
					Settings.SaveSetting("PathFFXI", PathFFXI.Text);
                    ConsoleLog($"Saved FFXI Path: {selectedPath}");
				}
				else
				{
					ConsoleLog("Error: Could not detect FFXI Folder, make sure it is your SquareEnix\\FINAL FANTASY XI Folder");
				}
			}
		}

		private void PathNoesis_SelectPath(object sender, MouseEventArgs e)
		{
			var dialog = new FolderBrowserDialog();
			dialog.Description = "Browse To your Noesis Folder";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
                string selectedPath = dialog.SelectedPath;
                string checkFilename = $"{selectedPath}\\Noesis64.exe";

                // Check noesis exists in the selected path
				if (File.Exists(checkFilename))
                {
					PathNoesis.Text = checkFilename;
					Settings.SaveSetting("PathNoesis", checkFilename);
					ConsoleLog($"Saved Noesis: {checkFilename}");
				}
                else
                {
                    ConsoleLog("Error: Could not find Noesis64.exe in the selected path, try again.");
                }	
			}
		}

		/// -----------------------------------------------------------------------------------------
		/// Send Key Logic Flows
		/// -----------------------------------------------------------------------------------------

		private string ExtractSkeleton = "";

		private void FlowIntroduction(string flowName)
        {
			ConsoleLog($"[[ {flowName} ]]");
			ConsoleLog("Data will be exported as FBX with PNG textures.");
			ConsoleLog("This will take some time. Please try not stop it!");
			ConsoleLog("If it crashes or you do stop it, start again to continue.");
			ConsoleLog($"Time Now: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
			ConsoleLog("Starting...");
			ConsoleLog(" ");
		}

        private void FlowFinished(string outputFolder)
        {
			// Stop thread, we're done
			ConsoleLog("[[ FINISHED ]]");
            ConsoleLog($"Saved all data to: {outputFolder}");
			StopActiveThread();
		}

		/// <summary>
		/// A test one
		/// </summary>
		private void TestExtract()
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

		/// <summary>
		/// Extract all zones
		/// </summary>
		private void HandleZonesExtract()
        {
            string jsonFile = $"ToolData\\zones_1.json";
            string outputFolder = "ToolOutput_Zones";
            string noesisArgs = NoesisArgsZone.Text.Trim();
			string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

			// Grab FFXI Path
			string ffxiPath = Settings.GetSetting("PathFFXI");

			FlowIntroduction("Zones");

			// Load the JSON file into a string
			ConsoleLog($"Parsing JSON: {jsonFile}");
			string json = File.ReadAllText(jsonFile);

			// Parse the JSON string into a JObject
			JObject jsonObj = JObject.Parse(json);

			// Loop through each property in the JObject
			foreach (JProperty property in jsonObj.Properties())
			{
				string key = property.Name;
				JArray value = (JArray)property.Value;

				// Loop through each item in the JArray
				foreach (string item in value)
				{
                    string expName = key;
                    string[] zoneData = item.Split('|');
                    string zoneName = zoneData[0];
                    string zonePath = zoneData[1];

                    // Set the path to the full name
                    zonePath = $"{ffxiPath}\\ROM{zonePath}.DAT";

                    // Set the save output
                    string saveTo = $"{outputFolder}\\{expName}\\{zoneName}";
					Directory.CreateDirectory(saveTo);

                    // Append the filename onto the save to
                    saveTo = $"{appDirectory}{saveTo}\\{zoneName}.fbx";

                    // check if the file already exists, if so, then skip!
                    if (File.Exists(saveTo))
                    {
						ConsoleLog($"-- Already Exists = {expName}: {zoneName}");
						continue;
                    }

                    // Build the Noesis cmode command
                    string command = $"\"{zonePath}\" \"{saveTo}\" {noesisArgs}";
                    ConsoleLog($"-- {expName}: {zoneName}");

                    // Run the cmode command
					RunNoesisCMode(command);
				}
			}

            FlowFinished(outputFolder);
		}

        /// <summary>
        /// Extract all armor
        /// </summary>
        
        private void HandleArmorExtract()
        {
			string jsonFile = $"ToolData\\gear_1.json";
			string outputFolder = "ToolOutput_Gear";
			string noesisArgs = NoesisArgsZone.Text.Trim();
			string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

			// Grab FFXI Path
			string ffxiPath = Settings.GetSetting("PathFFXI");

			FlowIntroduction("Armor");

			// Load the JSON
			ConsoleLog($"Parsing JSON: {jsonFile}");
			string json = File.ReadAllText(jsonFile);
			var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, dynamic>>>(json);

			// And we begin
			foreach (var race in data)
			{
				// eg: "Elvaan_Female", "Elvaan_Male", etc
				string raceName = race.Key;
                string raceSkeleton = race.Value["Skeleton"];

                // format skeleton into a filepath and set the global one for the open Dialog
                string raceSkeletonPath = $"{ffxiPath}\\ROM{raceSkeleton}.DAT";
				ExtractSkeleton = raceSkeletonPath;

				// loop through gear types
				foreach (var gear in (JObject)race.Value["Gear"])
                {
                    string slotName = gear.Key;

                    // loop through each item
                    foreach (string item in gear.Value)
                    {
						string[] itemData = item.Split('|');
						string itemName = itemData[0];
						string itemPath = itemData[1];

						// if the name is "None" we skip
						if (itemName == "None") { continue; }

						// Set the path to the full name
						itemPath = $"{ffxiPath}\\ROM{itemPath}.DAT";

						// Set the save output
						string saveTo = $"{outputFolder}\\{raceName}\\{slotName}";
						Directory.CreateDirectory(saveTo);

						// Append the filename onto the save to
						saveTo = $"{appDirectory}{saveTo}\\{itemName}.fbx";

						// check if the file already exists, if so, then skip!
						if (File.Exists(saveTo))
						{
							continue;
						}

						// Build the Noesis cmode command
						string command = $"\"{itemPath}\" \"{saveTo}\" {noesisArgs}";
						ConsoleLog($"--- {raceName} - {slotName} - {itemName}");

						// Run the cmode command
						RunNoesisCMode(command, 2);

						break;
                    }

                    break;
                }

                break;
			}

			FlowFinished(outputFolder);
		}

        /// <summary>
        /// Extract all weapons
        /// </summary>
		private void HandleWeaponsExtract()
        {
            string jsonFile = $"ToolData\\weapons_1.json";
            string outputFolder = "ToolOutput_Weapons";
            string noesisArgs = NoesisArgsZone.Text.Trim();
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Grab FFXI Path
            string ffxiPath = Settings.GetSetting("PathFFXI");

			FlowIntroduction("Weapons");

			// Load the JSON file into a string
			ConsoleLog($"Parsing JSON: {jsonFile}");
            string json = File.ReadAllText(jsonFile);
            JObject jsonObj = JObject.Parse(json);

            // Loop through each property in the JObject
            foreach (JProperty slotdata in jsonObj.Properties())
            {
                // eg: "Main", "Range", "Sub"
                string slotName = slotdata.Name;
                JObject slots = (JObject)slotdata.Value;

                // Loop through each item in the JArray
                foreach (JProperty typedata in slots.Properties())
                {
                    // eg: "Hand", "Dagger", "Sword" etc
                    string typeName = typedata.Name;
                    JArray weapons = (JArray)typedata.Value;

                    // if type is "None" SKIP
                    if (typeName == "None") { continue;  }

                    // Loop through each item in the JArray
                    foreach (string item in weapons)
                    {
                        string[] itemData = item.Split('|');
                        string itemName = itemData[0];
                        string itemPath = itemData[1];

						// Set the path to the full name
						itemPath = $"{ffxiPath}\\ROM{itemPath}.DAT";

                        // Set the save output
						string saveTo = $"{outputFolder}\\{slotName}\\{typeName}";
						Directory.CreateDirectory(saveTo);

						// Append the filename onto the save to
						saveTo = $"{appDirectory}{saveTo}\\{itemName}.fbx";

						// check if the file already exists, if so, then skip!
						if (File.Exists(saveTo))
						{
							continue;
						}

						// Build the Noesis cmode command
						string command = $"\"{itemPath}\" \"{saveTo}\" {noesisArgs}";
						ConsoleLog($"--- {slotName} - {typeName} - {itemName}");

						// Run the cmode command
						RunNoesisCMode(command, 1);
                    }
                }
            }

			FlowFinished(outputFolder);
		}

		/// <summary>
		/// Extract all animations
		/// </summary>
		private void HandleAnimationsExtract()
        {
			
		}

        private void HandleNpcMainExtract()
        {

        }

        private void HandleNpcLookExtract()
        {

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