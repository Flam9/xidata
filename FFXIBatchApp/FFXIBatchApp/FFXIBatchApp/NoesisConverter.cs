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
			}
		}

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

					// the save to folder
					string saveTo = $"{savepath2}\\Anims_{raceName}_Basic.ff11datset";
					File.WriteAllText(saveTo, contents);

					ConsoleLog($"-- {raceName} - Basic");

					results[raceName].Add($"Basic|{saveTo}");
				}
			}

			// Now build all other anims

			// Load the JSON
			string animJson = File.ReadAllText($"{savepath1}\\anims_1.json");
			var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, dynamic>>>(animJson);

		
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

						// the save to folder
						string saveTo = $"{savepath2}\\Anims_{raceName}_{animType}.ff11datset";
						File.WriteAllText(saveTo, contents);

						ConsoleLog($"-- {raceName} - {animType}");

						results[raceName].Add($"{animType}|{saveTo}");
					}
				}
			}

			string json = JsonConvert.SerializeObject(results, Formatting.Indented);
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
			File.WriteAllText(filePath, json);

			ConsoleLog("- BuildAnimDatSets Complete!");
		}
	}
}