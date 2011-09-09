using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    public class pageQuoteEditor: TabPagePrototype
    {
        ItWorks.RTFed rtfed = new ItWorks.RTFed();

        public pageQuoteEditor()
        {
            this.Text = "Редактор";
            //this.AddToolStripItem(new ToolStripButtonPrototype("Редактировать цитату", Resources._1315515132_edit));
            rtfed.RtfTextBox.Dock = DockStyle.Fill;
            this.Controls.Add(rtfed.RtfTextBox);
            this.Controls.Add(rtfed.RtfToolBar);
            this.Controls.Add(rtfed.StatusBar);
        }
    }
}
