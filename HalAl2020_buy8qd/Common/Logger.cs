using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace HalAl2020_buy8qd.Common
{
    public class Logger
    {
        private static string Logs = null;
        static Logger()
        {
            Logs = $"{Directory.GetCurrentDirectory()}\\Logs";
        }
        public static void Log(string text, string problem, string algo)
        {
            File.WriteAllText($"{Logs}\\{algo}_{problem}_{DateTime.Now.ToFileTime()}.log", text);
        }
    }
}
