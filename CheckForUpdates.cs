using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItWorksTeam.NET;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace QuotePad
{
    public class CheckUpdate
    {
        private string HostURL = "http://itworks.sytes.net/support.php?";

        private UpdateInformation CheckUpdates()
        {
            string myversion = new assembly().AssemblyVersion;
            string Response = "";
            HTTP request = new HTTP();
            request.Encoding = "UTF-8";
            request.ContentType = "text/xml";
            request.Timeout = 5; // 5 seconds timeout
            request.Method = HTTP.HttpMethod.GET;
            Response = request.CreateRequest(HostURL + "product=quotepad&action=CheckUpdates"+
                "&myversion="+myversion+"&username="+Environment.UserName);

            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(UpdateInformation));
                StringReader sr = new StringReader(Response);
                UpdateInformation update = (UpdateInformation)xs.Deserialize(sr);
                sr.Close();
                return update;
            }
            catch (InvalidOperationException ex)
            {
                InvalidOperationException aex = ex;
                return new UpdateInformation();
            }
        }

        public void CheckForUpdates()
        {
            CheckUpdate updates = new CheckUpdate();
            updates.CheckUpdates();
            assembly ass = new assembly();
            UpdateInformation update = updates.CheckUpdates();
            if (update.Product != null && update.Version.CompareTo(ass.AssemblyVersion) > 0)
            {
                bool IsCompatible = update.CompatibleVersions.Contains(ass.AssemblyVersion);
                if (IsCompatible)
                {
                    if (MessageBox.Show(string.Format("Доступна свежая версия программы {0} от {2}.\n"+
                        "Не желаете ли скачать её сейчас? \n\nЗаметки к выпуску:\n{1}",
                        update.Version, update.ReleaseNotes, update.Released.ToLongDateString() +
                        update.Released.ToShortTimeString()), "Доступно обновление", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        Process.Start("http://itworks.sytes.net/support.php?" +
                            "product=quotepad&action=Download" +
                            "&myversion=" + ass.AssemblyVersion + 
                            "&username=" + Environment.UserName);
                    }
                }
                else
                {
                    if (MessageBox.Show(string.Format("Доступна новая версия программы {0} от {2},"+
                        "которая НЕСОВМЕСТИМА с текущей!\nНе желаете скачать её сейчас? \n\nЗаметки к выпуску:\n{1}",
                        update.Version, update.ReleaseNotes, update.Released.ToLongDateString() +
                        update.Released.ToShortTimeString()), 
                        "Доступна новая версия программы", MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        Process.Start("http://itworks.sytes.net/support.php?" +
                            "product=quotepad&action=Download" +
                            "&myversion=" + ass.AssemblyVersion +
                            "&username=" + Environment.UserName);
                    }
                }
            }
        }
    }

    [Serializable]
    public class UpdateInformation
    {
        public string Product;
        public string Version;
        public string CompatibleVersions;
        public DateTime Released;
        public string ReleaseNotes;
    }
}
