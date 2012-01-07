using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace QuotePad
{
    public static class Program
    {
        static Mutex mutex;

        static bool IsNew()
        {
            try
            {
                mutex = Mutex.OpenExisting("QuotePad");
                return false;
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                mutex = new Mutex(false, "QuotePad");
                return true;
            }
        }

        [STAThread]
        public static void Main(string[] args)
        {
            //if (args != null && args.Length > 0)
            //{
            //    string argument = args[0].ToString();
            //    if (argument.Contains("="))
            //    {
            //        if (argument.Split('=').GetValue(0).ToString().ToLower() == "/setpassword")
            //        {
            //            if (argument.Split('=').GetValue(1).ToString().Length > 0)
            //            {
            //                new Authorization().SetNewPassword(argument.Split('=').GetValue(1).ToString());
            //                MessageBox.Show("Пароль установлен", new assembly().AssemblyProduct, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            }
            //        }
            //    }
            //}
            //else
            //{
                if (IsNew())
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    //Database.InitDb();
                    Thread t = new Thread(new CheckUpdate().CheckForUpdates);
                    t.Start();
                    Application.Run(new QuotePad.mainForm());
                    if (t.IsAlive)
                    {
                        t.Abort();
                    }
                    //Database.Disconnect();
                }
                else
                {
                    MessageBox.Show("Программа уже запущена!", new assembly().AssemblyProduct, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Application.Exit();
                    Application.ExitThread();
                }
            //}
        }
    }
}
