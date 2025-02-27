﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;


namespace CommandServer
{
    internal class Base
    {
        static TcpListener listener;
        static Socket socket;
        static NetworkStream stream;
        static StreamWriter writer;
        static StreamReader reader;
        static StringBuilder builder;
        static Thread th_StartListen, th_RunServer;

        public static Form1 frm = null;
        public static ToolStripStatusLabel tsl = null;
        public static TextBox tb1 = null;
        public static TextBox tb2 = null;
        
        private enum command
        {
            HELP = 1,
            MESSAGE = 2,
            BEEP = 3,
            SOUND = 4,
            SHUTDOWN = 5,
        }
        
        public static void init(Form1 frmIn, ToolStripStatusLabel tslIn, TextBox tb1In, TextBox tb2In)
        {
            frm = frmIn;
            tsl = tslIn;
            tb1 = tb1In;
            tb2 = tb2In;
            tb2.Focus();
        }

        public static void startThreads()
        {
            th_StartListen = new Thread(new ThreadStart(StartListen));
            th_StartListen.Start();
        }

        private static void StartListen()
        {
            listener = new TcpListener(System.Net.IPAddress.Any, 6666);
            listener.Start();
            tsl.Text = "Listening on port 6666...";
            for (; ; )
            {
                socket = listener.AcceptSocket();
                IPEndPoint ipend = (IPEndPoint)socket.RemoteEndPoint;
                tsl.Text = String.Format("Connection from {0}", IPAddress.Parse(ipend.Address.ToString()));
                th_RunServer = new Thread(new ThreadStart(RunServer));
                th_RunServer.Start();
            }
        }

        private static void RunServer()
        {
            stream = new NetworkStream(socket);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            builder = new StringBuilder();

            while (true)
            {
                try
                {
                    builder.Append(reader.ReadLine());
                    builder.Append("\r\n");
                    tb1.AppendText(builder.ToString());
                    builder.Remove(0, builder.Length);

                }
                catch (Exception e)
                {
                    //Cleanup();
                    //break;

                }
                Application.DoEvents();
            }
        }

        private static void Cleanup()
        {
            try
            {
                reader.Close();
                writer.Close();
                stream.Close();
                socket.Close();
            }
            catch (Exception e)
            {

            }
            tsl.Text = "Connection Lost";
        }

        public static void tb2_keydown(KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    builder.Append(tb2.Text.ToString());
                    writer.WriteLine(builder);
                    writer.Flush();
                    builder.Remove(0, builder.Length);
                    if (tb2.Text == "exit") Cleanup();
                    if (tb2.Text == "terminate") Cleanup();
                    if (tb2.Text == "cls") tb1.Text = "";
                    tb1.Text = "";
                }
            }
            catch (Exception err) { }
        }

        public static void btn1_Click()
        {
            writer.WriteLine("" + (int)command.MESSAGE);
            writer.Flush();
        }

        public static void btn2_Click()
        {
            writer.WriteLine("" + (int)command.BEEP);
            writer.Flush();
        }

        public static void btn3_Click()
        {
            writer.WriteLine("" + (int)command.SOUND);
            writer.Flush();
        }

        public static void btn4_Click()
        {
            writer.WriteLine("" + (int)command.SHUTDOWN);
            writer.Flush();
            tsl.Text = "Server has been shut down";
        }

        public static void frm_closing()
        {
            Cleanup();
            System.Environment.Exit(0);
        }
    }
}
