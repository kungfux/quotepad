using System;
using System.Collections.Generic;
using System.Linq;
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


        public static void Main(string[] args)
        {
            if (IsNew())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new QuotePad.mainForm());
            }
            else
            {
                MessageBox.Show("Программа уже запущена!", new assembly().AssemblyProduct, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
                Application.ExitThread();
            }

        }
    }
}
