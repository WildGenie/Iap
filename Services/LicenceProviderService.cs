using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iap.Services
{
    public class LicenceProviderService : ILicenceProviderService
    {
        public bool hasAlreadyKey()
        {
            try
            {
                string key = "Iap";
                RegistryKey keyToRetr = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + key);
                if (keyToRetr != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public string RetrieveTypeFromRegistry()
        {
            string key = "Iap";
            RegistryKey keyToRetr = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + key);
            if (keyToRetr != null)
            {
                return keyToRetr.GetValue("Type").ToString();
            }
            else
            {
                return "null";
            }
        }

        public void writeKeyToRegistry(string type)
        {
            try
            {
                string key = "IAP";
                    RegistryKey keyToRetr = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + key);
                    if (keyToRetr == null)
                    {
                        RegistryKey Regkey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\" + key);
                        Regkey.SetValue("Type", type);
                    }
                    
            }
            catch
            {
            }
        }
    }
}
