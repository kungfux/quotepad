using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    public class pageQuoteView : TabPagePrototype
    {
        ItWorks.RTFed rtfed = new ItWorks.RTFed();

        public pageQuoteView()
        {
            this.Text = "Просмотр";
            this.AddToolStripItem(new ToolStripButtonPrototype("Редактировать цитату", Resources._1315515132_edit));
            rtfed.RtfTextBox.Dock = DockStyle.Fill;
            rtfed.RtfTextBox.ReadOnly = true;
            this.Controls.Add(rtfed.RtfTextBox);
        }
    }
}
