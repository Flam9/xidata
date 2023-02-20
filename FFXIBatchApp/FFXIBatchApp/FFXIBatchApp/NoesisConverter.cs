using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace FFXIBatchApp
{
	internal class NoesisConverter
	{
		string savepath1 = "ToolData";
		string savepath2 = "ToolDataNoesis";

		public NoesisConverter() { }

		public void init()
		{
			ConsoleLog("Building Noesis Datsets Data");

			CheckDataFolderExists();

			// Build everything to make batch exports easier!
			BuildAnimDatSets();
			BuildNpcLookDatSets();
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
		/// Check that the AltanaData folder exists which is where we will save all our converted stuff
		/// </summary>
		private void CheckDataFolderExists()
		{
			string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, savepath2);

			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
				Directory.CreateDirectory($"{directoryPath}/Anims");
				Directory.CreateDirectory($"{directoryPath}/NPC");
			}
		}

		/// <summary>
		/// This loads up Anims and builds out a Noesis FF11 Datset file for each animation type.
		/// </summary>
		private void BuildAnimDatSets()
		{
			string filename = $"{savepath1}/anims_2.json";

			// Already built, skip.
			if (File.Exists(filename))
			{
				return;
			}

			ConsoleLog("- BuildAnimDatSets");

			// A list to save to so we can quickly process these in bulk
			Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();

			// Quickly build the basic anim sets as they just need to be copy/pasted.
			// Loop through resource names
			foreach (string raceName in FF11Race.GetRaces())
			{
				string ffxiPath = Settings.GetSetting("PathFFXI");
				string raceTemplate = $"FFXIBatchApp.Resources.DatsetAnims.Anims_{raceName}_Basic.ff11datset";

				// Load the template
				Assembly assembly = Assembly.GetExecutingAssembly();
				using (Stream stream = assembly.GetManifestResourceStream(raceTemplate))
				using (StreamReader reader = new StreamReader(stream))
				{
					// this is the template
					string contents = reader.ReadToEnd();

					// first replace the ffxipath
					contents = contents.Replace("[[FFXI_PATH]]", $"{ffxiPath}\\");

					if (!results.ContainsKey(raceName))
					{
						results[raceName] = new List<string>();
					}

					// All backslashes must be forward slashes
					contents = contents.Replace("\\", "/");

					// the save to path
					string saveTo = $"{savepath2}\\Anims\\{raceName}";
					Directory.CreateDirectory(saveTo);					
					saveTo = $"{saveTo}\\Basic.ff11datset";
					File.WriteAllText(saveTo, contents);

					ConsoleLog($"-- {raceName} - Basic");
					results[raceName].Add($"Basic|{saveTo}");
				}
			}

			// Now build all other anims

			// Load the JSON
			string jsonfile = File.ReadAllText($"{savepath1}\\anims_1.json");
			var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, dynamic>>>(jsonfile);

			// And we begin
			foreach (var race in data)
			{
				// eg: "Elvaan_Female", "Elvaan_Male", etc
				string raceName = race.Key;
				string raceTemplate = $"FFXIBatchApp.Resources.DatsetTemplate.Animations_{raceName}.template";
				string ffxiPath = Settings.GetSetting("PathFFXI");

				// Load the template
				Assembly assembly = Assembly.GetExecutingAssembly();
				using (Stream stream = assembly.GetManifestResourceStream(raceTemplate))
				using (StreamReader reader = new StreamReader(stream))
				{
					// this is the template
					string contents = reader.ReadToEnd();

					// first replace the ffxipath
					contents = contents.Replace("[[FFXI_PATH]]", $"{ffxiPath}\\");

					// loop through gear types
					foreach (var anims in race.Value)
					{
						string animType = anims.Key;
						string datsetLines = "";

						foreach (string anim in anims.Value)
						{
							string[] animData = anim.Split('|');
							string animName = animData[0];
							string animPath = animData[1];

							// make the Noesis Datset entry
							datsetLines += $"dat \"__animation\" \"ROM{animPath}.DAT\"\n";
						}

						// replace with the confirmed datswet lines
						contents = contents.Replace("[[ALL_OTHER_ANIMATIONS]]", datsetLines);

						// All backslashes must be forward slashes
						contents = contents.Replace("\\", "/");

						// the save to path
						string saveTo = $"{savepath2}\\Anims\\{raceName}";
						Directory.CreateDirectory(saveTo);
						saveTo = $"{saveTo}\\{animType}.ff11datset";
						results[raceName].Add($"{animType}|{saveTo}");

						if (File.Exists(saveTo))
						{
							continue;
						}

						File.WriteAllText(saveTo, contents);
						ConsoleLog($"-- {raceName} - {animType}");
					}
				}
			}

			string json = JsonConvert.SerializeObject(results, Formatting.Indented);
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
			File.WriteAllText(filePath, json);

			ConsoleLog("- BuildAnimDatSets Complete!");
		}

		/// <summary>
		/// This loads up the NPC2 (look data) and builds out Noesis FF11 Datset files for each NPC.
		/// </summary>
		private void BuildNpcLookDatSets()
		{
			string filename = $"{savepath1}/npc_3.json";

			// Already built, skip.
			if (File.Exists(filename))
			{
				return;
			}

			ConsoleLog("- BuildNpcLookDatSets");

			// A list to save to so we can quickly process these in bulk
			List<string> results = new List<string>();

			// Grab FFXI Path
			string ffxiPath = Settings.GetSetting("PathFFXI");

			// Load the JSON
			string jsonfile = File.ReadAllText($"{savepath1}\\npc_2.json");
			var data = JsonConvert.DeserializeObject<List<dynamic>>(jsonfile);
			
			// There are thousands of npcs, so we'll just note the total
			int buildCount = 0;

			// And we begin
			foreach (var npc in data)
			{
				// grab what we neeeed
				string npcRace = (string)npc.Race;

				string datset = "NOESIS_FF11_DAT_SET\n";
				datset += $"setPathAbs \"{ffxiPath}\\\"\n";

				// set skeleton
				string skeleton = npcRace.Split('|')[1];
				datset += $"dat \"__skeleton\" \"ROM{skeleton}.DAT\"\n";

				// Loop through parts
				List<string> slots = new List<string>() { "Face", "Head", "Body", "Hands", "Legs", "Feet", "Main", "Sub", "Range" };

				foreach (string slot in slots)
				{
					string slotdata = (string)npc[slot];

					// slot empty.
					if (slotdata.Length == 0) { continue; }

					// format: ID|ModelID|DatPath
					string datpath = slotdata.Split('|')[2];
					datset += $"dat \"{slot}\" \"ROM{datpath}.DAT\"\n";
				}

				// All backslashes must be forward slashes
				datset = datset.Replace("\\", "/");

				// the save to path
				string saveTo = $"{savepath2}\\NPC\\{npc.Zone}";
				Directory.CreateDirectory(saveTo);
				saveTo = $"{saveTo}\\{npc.ID}_{npc.Name}.ff11datset";
				results.Add($"{npc.ID}|{npc.Name}|{npc.Zone}|{saveTo}");

				if (File.Exists(saveTo))
				{
					continue;
				}

				File.WriteAllText(saveTo, datset);
				buildCount++;
			}

			ConsoleLog($"-- Built {buildCount} NPC Datsets!");

			string json = JsonConvert.SerializeObject(results, Formatting.Indented);
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
			File.WriteAllText(filePath, json);

			ConsoleLog("- BuildNpcLookDatSets Complete!");
		}
	}
}