using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace QuotePad
{
    public class ClientParams
    {
        public schemaClientParams clientParams { get; private set; }
        public ClientParams()
        {
            ItWorks.XML xml = new ItWorks.XML();
            schemaClientParams cp = xml.DeserializeClass<schemaClientParams>
                (Application.StartupPath + "\\config.xml");
            if (cp != null)
                clientParams = cp;
            else clientParams = new schemaClientParams();

            if (clientParams.HeaderColor == "") clientParams.HeaderColor = Color.AliceBlue.Name;
            if (clientParams.WorkingAreaColor == "") clientParams.WorkingAreaColor = Color.Aqua.Name;
        }

        public void Save()
        {
            ItWorks.XML xml = new ItWorks.XML();
            xml.SerializeClass<schemaClientParams>(clientParams,
                Application.StartupPath + "\\config.xml");
        }
    }

    public class schemaClientParams
    {
        public Size StartUpSize = new Size(800, 600);
        public string HeaderColor = Color.Honeydew.Name;
        public string WorkingAreaColor = Color.Indigo.Name;

        public string DatabaseFileName = "db.mdb";

    }
}
