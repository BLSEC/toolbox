﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotingInterface
{
    public interface DesktopInterface
    {
        string GetCommands();
        void SendKeystrokes(String keylog);
    }
}
