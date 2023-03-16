﻿using Newtonsoft.Json;
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
using Microsoft.VisualBasic.Devices;
using PlayOnline.FFXI;

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
			List<string> results = new List<string>();
			List<string> ff11datsets = new List<string>();
			string filename = $"{savepath1}/anims_2.txt";
			string ffxiPath = Settings.GetSetting("PathFFXI");

			ConsoleLog("- BuildAnimDatSets");

			// Quickly build the basic anim sets as they just need to be copy/pasted.
			// Loop through resource names
			foreach (string race in FF11Race.GetRaces())
			{
				// Load the template
				Assembly assembly = Assembly.GetExecutingAssembly();
				using (Stream stream = assembly.GetManifestResourceStream($"FFXIBatchApp.Resources.DatsetAnims.Anims_{race}_Basic.ff11datset"))
				using (StreamReader reader = new StreamReader(stream))
				{
					// this is the template
					string contents = reader.ReadToEnd();

					// first replace the ffxipath
					contents = contents.Replace("[[FFXI_PATH]]", $"{ffxiPath}\\");

					// All backslashes must be forward slashes
					contents = contents.Replace("\\", "/");

					// the save to path
					string saveTo = $"{savepath2}\\Anims\\{race}";
					Directory.CreateDirectory(saveTo);	
					
					// write out basic anims
					saveTo = $"{saveTo}\\Basic.ff11datset";
					ff11datsets.Add($"{race}|Basic|{saveTo}");

					if (File.Exists(saveTo)) { continue; }

					File.WriteAllText(saveTo, contents);

					// add to list
					ConsoleLog($"-- {race} - Basic");
					results.Add($"{race}|Basic|{saveTo}");
				}
			}

			// Now build all other anims

			// Load the JSON
			string[] animData = File.ReadAllText($"{savepath1}\\anims_1.txt").Split('\n');

			// loop through each line
			string groupCurrent = "";
			List<string> groupList = new List<string>();
			foreach (string animLine in animData)
			{
				string[] anim = animLine.Split('|');

				string race = anim[0];
				string group = anim[1];
				string name = anim[2];
				string dat = anim[3];

				// get the Noesis Template
				string raceTemplate = $"FFXIBatchApp.Resources.DatsetTemplate.Animations_{race}.template";

				if (groupCurrent != group && groupList.Count > 0)
				{
					// write out the file
					// Load the template
					Assembly assembly = Assembly.GetExecutingAssembly();
					using (Stream stream = assembly.GetManifestResourceStream(raceTemplate))
					using (StreamReader reader = new StreamReader(stream))
					{
						// this is the template
						string contents = reader.ReadToEnd();

						// first replace the ffxipath
						contents = contents.Replace("[[FFXI_PATH]]", $"{ffxiPath}\\");

						// replace with the confirmed datswet lines
						contents = contents.Replace("[[ALL_OTHER_ANIMATIONS]]", String.Join("\n", groupList));

						// All backslashes must be forward slashes
						contents = contents.Replace("\\", "/");

						// Save the ff11datset
						string saveTo = $"{savepath2}\\Anims\\{race}\\{groupCurrent}.ff11datset";
						ff11datsets.Add($"{race}|{groupCurrent}|{saveTo}");

						// clear datlist for the next loop
						groupList.Clear();

						if (!File.Exists(saveTo)) 
						{
							File.WriteAllText(saveTo, contents);
							ConsoleLog($"-- {race} - {groupCurrent}");
						}
					}
				}

				// Add to the list for this group
				groupList.Add($"dat \"__animation\" \"ROM{dat}.DAT\"");

				// Set group
				groupCurrent = group;
			}

			string fileData = String.Join("\n", ff11datsets.ToArray());
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
			File.WriteAllText(filePath, fileData);
			File.WriteAllText(filePath + ".total", $"{ff11datsets.Count}");
		}

		/// <summary>
		/// This loads up the NPC2 (look data) and builds out Noesis FF11 Datset files for each NPC.
		/// </summary>
		private void BuildNpcLookDatSets()
		{
			string filename = $"{savepath1}/npc_3.json";
			int total = 0;

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
				saveTo = $"{saveTo}\\{npc.UEID}.ff11datset";
				results.Add($"{npc.ID}|{npc.Name}|{npc.Zone}|{saveTo}");
				total++;

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
			File.WriteAllText(filePath + ".total", $"{total}");

			ConsoleLog("- BuildNpcLookDatSets Complete!");
		}
	}
}