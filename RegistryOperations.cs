using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace QuotePad
{
    public class RegistryOperations
    {
        public bool SaveKeyToRegistry(string path, string key, string pwdhash) // path like "Software\\ItWorks\\QuotePad", key like "Password", pwdhash - hash of the password
        {
            RegistryKey saveKey = Registry.LocalMachine.CreateSubKey(path);
            saveKey.SetValue(key, pwdhash);
            saveKey.Close();
            return true;
        }

        public string ReadKeyFromRegistry(string path, string key) // path like "Software\\ItWorks\\QuotePad", key like "Password"
        {
            RegistryKey readKey = Registry.LocalMachine.OpenSubKey(path);
            string valueFromRegistry = (string)readKey.GetValue(key);
            readKey.Close();
            return valueFromRegistry;
        }

        public bool DoesKeyAlreadyExist(string path, string key)
        {
            RegistryKey readKey = Registry.LocalMachine.OpenSubKey(path);
            string keyValue = (string)readKey.GetValue(key);
            readKey.Close();
            if (keyValue != null) return true;
            else return false;
        }
    }
}
