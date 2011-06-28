using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace QuotePad.GUI
{
    public partial class MainForm2: Form
    {
        public MainForm2()
        {
            this.Load += new EventHandler(MainForm2_Load);
        }

        void MainForm2_Load(object sender, EventArgs e)
        {
            Skins skins = new Skins();
            skins.ApplySkin("test", this);
        }
    }
}
