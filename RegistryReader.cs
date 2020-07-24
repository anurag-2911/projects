using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class RegistryReader
    {
        public static void ReadRegistry()
        {
            string regKey = @"SOFTWARE\Novell\ZCM\DisableWalMode";
            string value = "collectionStats";

            object result = ReadHKLMRegistry(regKey, value);
        }

        public static object ReadHKLMRegistry(string regkey, string value)
        {
            object registryValue = null;
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regkey))
                {
                    if (key != null)
                    {
                        registryValue = key.GetValue(value);

                    }
                }
            }
            catch (Exception ex)
            {
               
            }
            return registryValue;
        }
    }
}
