using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FFXIBatchApp
{
    static class Logger
	{
        private static List<string> lines = new List<string>();
        private static string logfile = "";

        public static void SetLogFile()
        {
            logfile = GetApplicationPath() + "/log.txt";
        }

        //
        // Add to log
        //
        public static void Add(string text)
        {
            // create log entry
            text = String.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), text);
            lines.Add(text);
        }

        public static List<String> Get()
        {
            return lines.ToList();
        }

        public static void Clear()
        {
            lines.Clear();
        }

        public static string GetApplicationPath()
        {
            string path = Assembly.GetExecutingAssembly().CodeBase;
            var directory = Path.GetDirectoryName(path);
            return new Uri(directory).LocalPath;
        }
    }
}
