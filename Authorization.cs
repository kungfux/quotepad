using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Windows.Forms;

namespace QuotePad
{
    public enum UserType
    {
        Editor = 0,
        Viewer = 1
    }

    class Authorization
    {
        private string PasswordRegistryPath = "Software\\ItWorksTeam\\QuotePad";
        private string PasswordRegistryKey = "Password";
        public static UserType userType = UserType.Viewer;
        ItWorks.Registry regedit = new ItWorks.Registry();

        public void SetNewPassword(string NewPassword)
        {
            string password = NewPassword;
            MD5CryptoServiceProvider Myhash = new MD5CryptoServiceProvider();
            byte[] bytetohash = Encoding.Unicode.GetBytes(password.ToString());
            Myhash.ComputeHash(bytetohash);
            string passHashPass = Convert.ToBase64String(Myhash.Hash);
            //MessageBox.Show("HASH: " + passHashPass);

            regedit.SaveKey(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE,
                PasswordRegistryPath, PasswordRegistryKey, passHashPass);
        }

        public bool ChangeExistingPassword(string OldPassword, string NewPassword)
        {
            string password_old = OldPassword;
            string password_new = NewPassword;

            //compute hash of old password to compare it with registry value for valideness
            MD5CryptoServiceProvider Myhash1 = new MD5CryptoServiceProvider();
            byte[] bytetohash1 = Encoding.Unicode.GetBytes(password_old.ToString());
            Myhash1.ComputeHash(bytetohash1);
            string hashPass_old = Convert.ToBase64String(Myhash1.Hash);

            //compute hash of new password
            MD5CryptoServiceProvider Myhash2 = new MD5CryptoServiceProvider();
            byte[] bytetohash2 = Encoding.Unicode.GetBytes(password_new.ToString());
            Myhash2.ComputeHash(bytetohash2);
            string hashPass_new = Convert.ToBase64String(Myhash2.Hash);

            if (GetPasswordFromRegistry() == hashPass_old)
            {
                return regedit.SaveKey(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE,
                    PasswordRegistryPath, PasswordRegistryKey, hashPass_new);
            }
            else
            { 
                // incorrect old password
                return false;
            }
        }

        private string GetPasswordFromRegistry()
        {
            //getting hash value of password from registry
            return regedit.ReadKey<string>(ItWorks.Registry.BaseKeys.HKEY_LOCAL_MACHINE,
                PasswordRegistryPath, PasswordRegistryKey, "");
        }

        public UserType CheckCredentials(string EnteredPassword)
        {
            //compute hash of entered password to compare it with hash value stored in registry for valideness
            MD5CryptoServiceProvider Myhash1 = new MD5CryptoServiceProvider();
            byte[] bytetohash1 = Encoding.Unicode.GetBytes(EnteredPassword.ToString());
            Myhash1.ComputeHash(bytetohash1);
            string hash_of_entered_password = Convert.ToBase64String(Myhash1.Hash);

            //getting hash value from registry
            if (GetPasswordFromRegistry() == hash_of_entered_password)
            {
                return UserType.Editor; // if entered password is correct
            }
            else
            {
                return UserType.Viewer; // if entered password is not correct
            }
        }

        // Perform auto-authorization if default password is set up
        public void AutoAuthorization()
        {
            userType = CheckCredentials("password");
        }
    }
}
