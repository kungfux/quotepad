using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace QuotePad
{
    static class Program
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
        static void Main()
        {
            if (IsNew())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                GUI.MainForm mainForm = new GUI.MainForm();
                Application.Run(new GUI.MainForm());
            }
            else
            {
                MessageBox.Show("Программа уже запущена!", "Цитатник", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
                Application.ExitThread();
            }
        }
    }
}
