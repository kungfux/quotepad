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
        private string HostURL = "http://itworks.sytes.net/products/QuotePad/update.xml";

        private Update CheckUpdates()
        {
            string Response = "";
            HTTP request = new HTTP();
            request.Encoding = "UTF-8";
            request.ContentType = "text/xml";
            request.Timeout = 5000;
            request.Method = HTTP.HttpMethod.GET;
            Response = request.CreateRequest(HostURL);

            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Update));
                StringReader sr = new StringReader(Response);
                Update update = (Update)xs.Deserialize(sr);
                sr.Close();
                return update;
            }
            catch (InvalidOperationException)
            {
                return new Update();
            }
        }

        public void CheckForUpdates()
        {
            CheckUpdate updates = new CheckUpdate();
            updates.CheckUpdates();
            assembly ass = new assembly();
            Update update = updates.CheckUpdates();
            if (update != null && update.ProductName == ass.AssemblyProduct && update.ProductVersion != ass.AssemblyVersion)
            {
                if (MessageBox.Show(string.Format("Доступна свежая версия программы {0}. Не желаете ли скачать её сейчас? \n\nЗаметки к выпуску:\n{1}",
                    update.ProductVersion, update.ReleaseNotes), "Доступно обновление", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Process.Start(update.UrlToDownload);
                }
            }
        }
    }

    [Serializable]
    public class Update
    {
        public string ProductName;
        public string ProductVersion;
        public string UrlToDownload;
        public string ReleaseNotes;
    }
}
